using DevAI.Application.Agents;
using DevAI.Application.AI;
using DevAI.Application.Chat;
using DevAI.Application.Memory;
using DevAI.Application.Planner;
using DevAI.Application.Tools;
using DevAI.Infrastructure.AI;
using DevAI.Infrastructure.Data;
using DevAI.Plugins.Memory;
using DevAI.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.Configure<AzureOpenAIOptions>(
//    builder.Configuration.GetSection("AzureOpenAI"));
builder.Services.AddOptions<AzureOpenAIOptions>()
    .Bind(builder.Configuration.GetSection("AzureOpenAI"))
    .ValidateOnStart();
/* =====================================================
   KERNEL (CRITICAL FIX)
===================================================== */

//var config = builder.Configuration.GetSection("AzureOpenAI");

//var endpoint = config["Endpoint"];
//var apiKey = config["ApiKey"];
//var chatDeployment = config["ChatDeployment"];
//var embeddingDeployment = config["EmbeddingDeployment"];
builder.Services.AddSingleton<Kernel>(sp =>
{
    var options = sp.GetRequiredService<IOptions<AzureOpenAIOptions>>().Value;

    var kernelBuilder = Kernel.CreateBuilder();

    kernelBuilder.AddAzureOpenAIChatCompletion(
        deploymentName: options.ChatDeployment,
        endpoint: options.Endpoint,
        apiKey: options.ApiKey);

    kernelBuilder.AddAzureOpenAITextEmbeddingGeneration(
        deploymentName: options.EmbeddingDeployment,
        endpoint: options.Endpoint,
        apiKey: options.ApiKey);

    return kernelBuilder.Build();
});

/* =====================================================
   CORE SERVICES
===================================================== */

builder.Services.AddSingleton<IKernelService, KernelService>();

builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseInMemoryDatabase("JarvisMemory"));

builder.Services.AddScoped<IMemoryService, MemoryService>();
builder.Services.AddScoped<IChatService, ChatService>();

builder.Services.AddSingleton<IEmbeddingService, EmbeddingService>();

builder.Services.AddScoped<MemoryExtractorService>();
builder.Services.AddScoped<MemoryContextService>();
builder.Services.AddScoped<UserProfileBuilderService>();
builder.Services.AddScoped<MemoryNormalizer>();

builder.Services.AddScoped<PlannerService>();

/* =====================================================
   AGENTS (FIXED LIFETIME STRATEGY)
===================================================== */

// Tool system MUST be scoped
builder.Services.AddScoped<IAgentTool, MemoryTool>();
builder.Services.AddScoped<ToolRegistry>();
builder.Services.AddScoped<ToolExecutorService>();

builder.Services.AddScoped<IntentService>();

// ⚠️ IMPORTANT: DO NOT USE AgentRouterService yet if circular issues exist
builder.Services.AddScoped<AgentRouterService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

/* =====================================================
   API
===================================================== */

app.MapGet("/chat", async (string message, IChatService chat) =>
{
    if (string.IsNullOrWhiteSpace(message))
        return Results.BadRequest("Message cannot be empty");

    var response = await chat.AskAsync(message);

    return Results.Ok(response);
});

app.Run();