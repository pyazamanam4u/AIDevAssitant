namespace DevAI.Domain.Agents;

public class IntentResult
{
    public IntentType Intent { get; set; }
    public string RawQuery { get; set; } = string.Empty;
}