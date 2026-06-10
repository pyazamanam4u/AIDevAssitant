using DevAI.Application.AI;
using DevAI.Application.Memory;
using Microsoft.SemanticKernel;

namespace DevAI.Application.Planner;

public class PlannerService
{
    private readonly IKernelService _kernelService;
    private readonly IMemoryService _memory;
    private readonly IEmbeddingService _embedding;

    public PlannerService(
        IKernelService kernelService,
        IMemoryService memory,
        IEmbeddingService embedding)
    {
        _kernelService = kernelService;
        _memory = memory;
        _embedding = embedding;
    }

    public async Task<string> GenerateDailyPlanAsync(string message)
    {
        var kernel = _kernelService.GetKernel();

        var queryEmbedding = await _embedding.GenerateEmbeddingAsync(message);

        var memories = await _memory.SearchAsync(queryEmbedding, 5);

        var context = string.Join("\n", memories.Select(m =>
            $"- {m.Type}:{m.SubType}:{m.Value}"));

        var prompt = $"""
You are an AI planner.

User context:
{context}

User request:
{message}

Return structured daily plan.
""";

        var result = await kernel.InvokePromptAsync(prompt);

        return result.ToString();
    }
}