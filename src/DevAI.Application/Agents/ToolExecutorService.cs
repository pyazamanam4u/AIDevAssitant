using DevAI.Application.Tools;
using DevAI.Domain.Agents;
using System.Numerics;

namespace DevAI.Application.Agents;

public class ToolExecutorService
{
    private readonly ToolRegistry _registry;

    public ToolExecutorService(ToolRegistry registry)
    {
        _registry = registry;
    }

    public async Task<string> ExecuteAsync(AgentPlan plan)
    {
        var results = new List<string>();

        foreach (var step in plan.Steps)
        {
            var tool = _registry.Get(step.Tool);

            if (tool == null)
            {
                results.Add($"Tool not found: {step.Tool}");
                continue;
            }

            var output = await tool.ExecuteAsync(step.Input);
            results.Add(output);
        }

        return string.Join("\n", results);
    }
}