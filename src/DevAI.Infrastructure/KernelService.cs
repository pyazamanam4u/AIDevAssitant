using Microsoft.SemanticKernel;
using Microsoft.Extensions.Options;
using DevAI.Shared;
using DevAI.Application.AI;
using DevAI.Plugins.Math;

namespace DevAI.Infrastructure.AI;

public class KernelService : IKernelService
{
    private readonly AzureOpenAIOptions _options;

    public KernelService(IOptions<AzureOpenAIOptions> options)
    {
        _options = options.Value;
    }


    public Kernel GetKernel()
    {
        var builder = Kernel.CreateBuilder();

        builder.AddAzureOpenAIChatCompletion(
            deploymentName: _options.ChatDeployment,
            endpoint: _options.Endpoint,
            apiKey: _options.ApiKey
        );
        builder.AddAzureOpenAITextEmbeddingGeneration(
    deploymentName: _options.EmbeddingDeployment,
    endpoint: _options.Endpoint,
    apiKey: _options.ApiKey);

        var kernel = builder.Build();

        // REGISTER PLUGIN
        kernel.Plugins.AddFromType<MathPlugin>("Math");

        return kernel;
    }
}