using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;

namespace DevAI.SemanticKernel;

public class KernelFactory
{
    public static Kernel Create(
        string endpoint,
        string apiKey,
        string deployment)
    {
        var builder = Kernel.CreateBuilder();

        builder.AddAzureOpenAIChatCompletion(
            deploymentName: deployment,
            endpoint: endpoint,
            apiKey: apiKey);

        builder.AddAzureOpenAITextEmbeddingGeneration(
            deploymentName: deployment, // IMPORTANT: must be embedding model
            endpoint: endpoint,
            apiKey: apiKey);

        return builder.Build();
    }
}