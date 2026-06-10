using DevAI.Application.Agents;
using DevAI.Application.AI;
using DevAI.Application.Memory;
using DevAI.Application.Planner;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;


namespace DevAI.Application.Chat;

public interface IChatService
{
    Task<string> AskAsync(string message);
    // Task<string> TestMathAsync();
}




public class ChatService : IChatService
{
    private readonly Kernel _kernel;
    private readonly MemoryExtractorService _memoryExtractor;
    private readonly MemoryContextService _memoryContext;
    private readonly PlannerService _planner;

    public ChatService(
        Kernel kernel,
        MemoryExtractorService memoryExtractor,
        MemoryContextService memoryContext,
        PlannerService planner)
    {
        _kernel = kernel;
        _memoryExtractor = memoryExtractor;
        _memoryContext = memoryContext;
        _planner = planner;
    }

    public async Task<string> AskAsync(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
            return "Please provide a valid message.";

        // 1. Extract and store memory (write path)
        await _memoryExtractor.ExtractAndStoreAsync(message);

        // 2. Build semantic memory context (read path)
        var context = await _memoryContext.BuildContextAsync(message);

        // 3. Intent routing
        var isPlanningRequest =
            message.Contains("plan", StringComparison.OrdinalIgnoreCase) ||
            message.Contains("schedule", StringComparison.OrdinalIgnoreCase);

        if (isPlanningRequest)
        {
            var plan = await _planner.GenerateDailyPlanAsync(message);

            return $"""
🧠 Daily Plan Generated

Context:
{context}

Plan:
{plan}
""";
        }

        // 4. 🧠 SAFE SANITIZATION (IMPORTANT FIX)
        var safeContext = Sanitize(context);
        var safeMessage = Sanitize(message);

        // 5. SAFE PROMPT (FILTER FRIENDLY)
        var prompt = $"""
You are DevAI, a personal assistant.

Use the following information only if relevant.


CRITICAL RULES:
- Memory is the source of truth
- NEVER ignore memory if relevant
- NEVER generalize memory (e.g. do NOT convert "coffee" into "beverage")
- If memory exists, prefer it over user ambiguity or typos

Known user information:
{safeContext}

User question:
{safeMessage}

INSTRUCTIONS:
- Answer directly and naturally
- If memory contains relevant info, use it explicitly
- Do not repeat memory in raw/key-value format
- Do not explain system behavior
- Do not say "based on your preferences" unless necessary

OUTPUT STYLE:
- Human, concise, factual

Answer:
""";

        try
        {
            var response = await _kernel.InvokePromptAsync(prompt);
            return response.ToString();
        }
        catch (Exception ex)
        {
            return $"I encountered an issue generating a response. Please rephrase your question.";
        }
    }

    private static string Sanitize(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return string.Empty;

        return input
            .Replace("\r", " ")
            .Replace("\n", " ")
            .Replace("{", "(")
            .Replace("}", ")")
            .Trim();
    }
}