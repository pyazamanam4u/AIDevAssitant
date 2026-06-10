namespace DevAI.Domain.Memory;

public class UserProfile
{
    public string Job { get; set; } = "unknown";
    public string Habit { get; set; } = "unknown";
    public string Routine { get; set; } = "unknown";

    public List<string> Likes { get; set; } = new();
    public List<string> Preferences { get; set; } = new();
}