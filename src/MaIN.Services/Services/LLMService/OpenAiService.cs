using MaIN.Domain.Configuration;
using MaIN.Services.Services.Abstract;
using Microsoft.Extensions.Logging;
using MaIN.Services.Services.LLMService.Memory;

namespace MaIN.Services.Services.LLMService;

public sealed class OpenAiService(
    MaINSettings settings,
    INotificationService notificationService,
    IHttpClientFactory httpClientFactory,
    IMemoryFactory memoryFactory,
    IMemoryService memoryService,
    ILogger<OpenAiService>? logger = null)
    : OpenAiCompatibleService(notificationService, httpClientFactory, memoryFactory, memoryService, logger)
{
    private readonly MaINSettings _settings = settings ?? throw new ArgumentNullException(nameof(settings));
      
    protected override string GetApiKey()
    {
        return _settings.OpenAiKey ?? Environment.GetEnvironmentVariable("OPENAI_API_KEY") ?? 
               throw new InvalidOperationException("OpenAi Key not configured");
    }

    protected override void ValidateApiKey()
    {
        if (string.IsNullOrEmpty(_settings.OpenAiKey) && string.IsNullOrEmpty(Environment.GetEnvironmentVariable("OPENAI_API_KEY")))
        {
            throw new InvalidOperationException("OpenAi Key not configured");
        }
    }
}
