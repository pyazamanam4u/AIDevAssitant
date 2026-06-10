using DevAI.Domain.Agents;
using DevAI.Application.AI;
using Microsoft.SemanticKernel;

namespace DevAI.Application.Agents;

public class IntentService
{
    private readonly IKernelService _kernelService;

    public IntentService(IKernelService kernelService)
    {
        _kernelService = kernelService;
    }

    public async Task<IntentResult> DetectAsync(string message)
    {
        var kernel = _kernelService.GetKernel();

        var prompt = $@"
Classify intent:
Chat, Math, Calendar, Email, MemoryQuery

Message:
{message}

Return only one word.
";

        var result = await kernel.InvokePromptAsync(prompt);

        var intentText = result.ToString()?.Trim().ToLower();

        IntentType intent = intentText switch
        {
            "math" => IntentType.Math,
            "calendar" => IntentType.Calendar,
            "email" => IntentType.Email,
            "memoryquery" => IntentType.MemoryQuery,
            _ => IntentType.Chat
        };

        return new IntentResult
        {
            Intent = intent,
            RawQuery = message
        };


    }
}