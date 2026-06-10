using DevAI.Application.Tools;
using DevAI.Application.Memory;
using DevAI.Application.AI;

namespace DevAI.Plugins.Memory;

public class MemoryTool : IAgentTool
{
    public string Name => "memory";

    private readonly IEmbeddingService _embedding;
    private readonly IMemoryService _memory;

    public MemoryTool(
        IEmbeddingService embedding,
        IMemoryService memory)
    {
        _embedding = embedding;
        _memory = memory;
    }

    public async Task<string> ExecuteAsync(string input)
    {
        var queryEmbedding = await _embedding.GenerateEmbeddingAsync(input);

        var memories = await _memory.SearchAsync(queryEmbedding);

        return string.Join("\n",
            memories.Select(m => $"{m.Type}/{m.SubType}: {m.Value}"));
    }
}