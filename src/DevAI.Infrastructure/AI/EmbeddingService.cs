using DevAI.Application.AI;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Embeddings;

public class EmbeddingService : IEmbeddingService
{
    private readonly Kernel _kernel;

    public EmbeddingService(Kernel kernel)
    {
        _kernel = kernel;
    }

    public async Task<float[]> GenerateEmbeddingAsync(string text)
    {
        var service = _kernel.GetRequiredService<ITextEmbeddingGenerationService>();
        var result = await service.GenerateEmbeddingAsync(text);

        return result.ToArray();
    }

    public async Task<string> GetChatResultAsync(string prompt)
    {
        var result = await _kernel.InvokePromptAsync(prompt);
        return result.ToString();
    }
}