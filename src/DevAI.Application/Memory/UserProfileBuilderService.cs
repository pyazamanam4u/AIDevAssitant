using DevAI.Domain.Memory;

namespace DevAI.Application.Memory;

public class UserProfileBuilderService
{
    private readonly IMemoryService _memory;
    private readonly MemoryNormalizer _normalizer;

    public UserProfileBuilderService(
        IMemoryService memory,
        MemoryNormalizer normalizer)
    {
        _memory = memory;
        _normalizer = normalizer;
    }

    public async Task<UserProfile> BuildAsync()
    {
        var memories = await _memory.GetAllAsync();

        var profile = new UserProfile();

        foreach (var m in memories)
        {
            switch (m.Type)
            {
                case "job":
                    profile.Job = m.Value;
                    break;

                case "habit":
                    profile.Habit = m.Value;
                    break;

                case "routine":
                    profile.Routine = m.Value;
                    break;

                case "preference":
                    profile.Preferences.Add($"{m.SubType}:{m.Value}");
                    break;
            }
        }

        return profile;
    }
}