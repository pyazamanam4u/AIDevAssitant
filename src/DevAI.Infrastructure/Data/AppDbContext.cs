
using DevAI.Domain.Memory;
using Microsoft.EntityFrameworkCore;

namespace DevAI.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<MemoryItem> StructuredMemories { get; set; }
}