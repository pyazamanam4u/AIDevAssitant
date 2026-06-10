using DevAI.Domain.Memory;

namespace DevAI.Application.Memory;

public class MemoryDto
{
    public string? job { get; set; }
    public string? habit { get; set; }
    public string? routine { get; set; }

    public List<PreferenceDto> preferences { get; set; } = new();
}

public class PreferenceDto
{
    public string type { get; set; } = "";
    public string value { get; set; } = "";
}


public class MemoryNormalizer
{
    public (string type, string value) Normalize(string type, string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return (type, value);

        value = value.Trim().ToLowerInvariant();

        return type switch
        {
            "drink" => ("preference", NormalizeDrink(value)),
            "food" => ("preference", value),
            "job" => ("job", value),
            "habit" => ("habit", value),
            _ => (type, value)
        };
    }

    private string NormalizeDrink(string value)
    {
        if (value.Contains("coffee")) return "coffee";
        if (value.Contains("tea")) return "tea";
        if (value.Contains("juice")) return "juice";

        return value;
    }

    private void Apply(UserProfile profile, string key, string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return;

        // JOB
        if (key.Contains("job"))
            profile.Job = value;

        // HABIT
        else if (key.Contains("habit"))
            profile.Habit = value;

        // ROUTINE
        else if (key.Contains("routine"))
            profile.Routine = value;

        // DRINK / FOOD / PREFERENCES
        else if (key.Contains("drink") || key.Contains("food"))
            profile.Preferences.Add($"{key}:{value}");

        // LIKES
        else if (key.Contains("like"))
            profile.Likes.Add(value);
    }
}