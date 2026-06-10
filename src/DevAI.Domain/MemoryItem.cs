public class MemoryItem
{
    public Guid Id { get; set; } = Guid.NewGuid();

    // Semantic classification (core intelligence layer)
    public string Type { get; set; } = string.Empty;        // job, habit, preference, routine
    public string SubType { get; set; } = string.Empty;     // drink, food, work, etc.

    // Actual extracted value
    public string Value { get; set; } = string.Empty;

    // Optional reasoning / extraction confidence
    public double Confidence { get; set; } = 1.0;

    // Vector embedding for semantic search
    public float[] Embedding { get; set; } = Array.Empty<float>();

    // Lifecycle control (VERY IMPORTANT for evolution later)
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public bool IsActive { get; set; } = true;
}