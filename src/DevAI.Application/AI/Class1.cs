namespace DevAI.Application.AI;

public interface IEmbeddingService
{
    Task<float[]> GenerateEmbeddingAsync(string text);

    Task<string> GetChatResultAsync(string prompt);
}
