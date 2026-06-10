using DevAI.Domain.Memory;
using System.Text.Json;
using DevAI.Application.AI;

namespace DevAI.Application.Memory;

public class MemoryExtractorService
{
    private readonly IEmbeddingService _embedding;
    private readonly IMemoryService _memory;
    private readonly MemoryNormalizer _normalizer = new();

    public MemoryExtractorService(IEmbeddingService embedding, IMemoryService memory, MemoryNormalizer normalizer)
    {
        _embedding = embedding;
        _memory = memory;
        _normalizer = normalizer;
    }

    public async Task ExtractAndStoreAsync(string message)
    {
        var embedding = await _embedding.GenerateEmbeddingAsync(message);

        var prompt = """
Extract structured memory ONLY in JSON:

{
  "job": "",
  "habit": "",
  "routine": "",
  "preferences": [
    { "type": "drink", "value": "" },
    { "type": "food", "value": "" }
  ]
}

Message:
""" + message;

        var result = await _embedding.GetChatResultAsync(prompt);

        var json = result.Trim('`', ' ', '\n');

        var data = JsonSerializer.Deserialize<MemoryDto>(json);
        if (data == null) return;

        // NORMALIZER
        foreach (var p in data.preferences)
        {
            var (type, value) = _normalizer.Normalize(p.type, p.value);

            if (!string.IsNullOrWhiteSpace(value))
            {
                await _memory.UpsertAsync(new MemoryItem
                {
                    Type = type,
                    SubType = p.type,
                    Value = value,
                    Embedding = embedding
                });
            }
        }

        if (!string.IsNullOrWhiteSpace(data.job))
        {
            var (type, value) = _normalizer.Normalize("job", data.job);

            await _memory.UpsertAsync(new MemoryItem
            {
                Type = type,
                Value = value,
                Embedding = embedding
            });
        }

        if (!string.IsNullOrWhiteSpace(data.habit))
        {
            var (type, value) = _normalizer.Normalize("habit", data.habit);

            await _memory.UpsertAsync(new MemoryItem
            {
                Type = type,
                Value = value,
                Embedding = embedding
            });
        }
    }
}