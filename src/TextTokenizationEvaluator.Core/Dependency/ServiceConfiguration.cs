using Microsoft.Extensions.DependencyInjection;
using TextTokenizationEvaluator.Core.Services;
using TextTokenizationEvaluator.Core.Services.Contracts;

namespace TextTokenizationEvaluator.Core.Dependency;

public static class ServiceConfiguration
{
    public static IServiceCollection AddTextTokenizationEvaluatorServices(this IServiceCollection services)
    {
        services.AddSingleton<BaseTokenizerProvider, TokenizerProviderService>();
        services.AddSingleton<IEncodingService, EncodingService>();
        services.AddSingleton<IModelEncodingProvider, ModelEncodingProvider>();
        return services;
    }
}