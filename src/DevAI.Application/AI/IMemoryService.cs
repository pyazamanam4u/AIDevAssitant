
using DevAI.Domain.Memory;

namespace DevAI.Application.Memory;

public interface IMemoryService
{
    Task UpsertAsync(MemoryItem item);

    Task<List<MemoryItem>> GetAllAsync();

    Task<List<MemoryItem>> SearchAsync(float[] queryEmbedding, int topK = 5);
}