using DevAI.Domain.Memory;
using DevAI.Application.Memory;
using Microsoft.EntityFrameworkCore;

namespace DevAI.Infrastructure.Data;

public class MemoryService : IMemoryService
{
    private readonly AppDbContext _db;

    public MemoryService(AppDbContext db)
    {
        _db = db;
    }

    public async Task UpsertAsync(MemoryItem item)
    {
        var existing = await _db.StructuredMemories
            .FirstOrDefaultAsync(x =>
                x.Type == item.Type &&
                x.SubType == item.SubType);

        if (existing == null)
        {
            _db.StructuredMemories.Add(item);
        }
        else
        {
            existing.Value = item.Value;
            existing.Embedding = item.Embedding;
            existing.UpdatedAt = DateTime.UtcNow;
        }

        await _db.SaveChangesAsync();
    }

    public async Task<List<MemoryItem>> GetAllAsync()
    {
        return await _db.StructuredMemories
            .Where(x => x.IsActive)
            .ToListAsync();
    }

    public async Task<List<MemoryItem>> SearchAsync(float[] queryEmbedding, int topK = 5)
    {
        return await _db.StructuredMemories
            .Where(x => x.IsActive)
            .OrderByDescending(x =>
                CosineSimilarity(x.Embedding, queryEmbedding))
            .Take(topK)
            .ToListAsync();
    }

    private double CosineSimilarity(float[] a, float[] b)
    {
        double dot = 0, magA = 0, magB = 0;

        for (int i = 0; i < a.Length; i++)
        {
            dot += a[i] * b[i];
            magA += a[i] * a[i];
            magB += b[i] * b[i];
        }

        return dot / (Math.Sqrt(magA) * Math.Sqrt(magB) + 1e-8);
    }
}