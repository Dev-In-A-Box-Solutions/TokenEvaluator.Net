using Microsoft.Extensions.DependencyInjection;
using TokenEvaluator.Net.Services;
using TokenEvaluator.Net.Services.Contracts;

namespace TokenEvaluator.Net.Dependency;

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