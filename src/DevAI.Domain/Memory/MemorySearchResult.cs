namespace DevAI.Domain.Memory;

public class MemorySearchResult
{
    // Same logical key from MemoryItem
    public string Key { get; set; } = string.Empty;

    // Stored value
    public string Value { get; set; } = string.Empty;

    // Final computed relevance score (AI ranking)
    public double Score { get; set; }

    // When memory was created (used for recency scoring in UI/debug)
    public DateTime CreatedAt { get; set; }

    // Optional: helps debugging ranking decisions
    public double Similarity { get; set; }

    public double RecencyScore { get; set; }

    public double Importance { get; set; }
}