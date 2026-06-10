namespace DevAI.Application.Tools;

public class ToolRegistry
{
    private readonly Dictionary<string, IAgentTool> _tools;

    public ToolRegistry(IEnumerable<IAgentTool> tools)
    {
        _tools = tools.ToDictionary(t => t.Name.ToLower());
    }

    public IAgentTool? Get(string name)
    {
        return _tools.TryGetValue(name.ToLower(), out var tool)
            ? tool
            : null;
    }
}