namespace DevAI.Domain.Agents;

public class AgentPlan
{
    public string Goal { get; set; } = string.Empty;

    public List<AgentStep> Steps { get; set; } = new();
}

public class AgentStep
{
    public string Tool { get; set; } = string.Empty;

    public string Input { get; set; } = string.Empty;
}