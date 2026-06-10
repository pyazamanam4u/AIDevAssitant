using DevAI.Application.AI;
using DevAI.Application.Memory;

public class MemoryContextService
{
    private readonly IMemoryService _memory;
    private readonly IEmbeddingService _embedding;

    public MemoryContextService(IMemoryService memory, IEmbeddingService embedding)
    {
        _memory = memory;
        _embedding = embedding;
    }

    public async Task<string> BuildContextAsync(string message)
    {
        var embedding = await _embedding.GenerateEmbeddingAsync(message);

        var memories = await _memory.SearchAsync(embedding, 5);

        if (memories.Count == 0)
            return string.Empty;

        return string.Join("\n",
    memories.Select(m =>
        $"User {m.Type} is {m.Value}."));
    }
}