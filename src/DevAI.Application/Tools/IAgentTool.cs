namespace DevAI.Application.Tools;

public interface IAgentTool
{
    string Name { get; }
    Task<string> ExecuteAsync(string input);
}