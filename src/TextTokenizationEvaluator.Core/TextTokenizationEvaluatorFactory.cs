using TextTokenizationEvaluator.Core.Services;
using TextTokenizationEvaluator.Core.Services.Contracts;

namespace TextTokenizationEvaluator.Core;

/// <summary>
/// This is a concrete, tightly-coupled factory class that creates a TokenEvaluatorClient instance.
/// </summary>
public static class TokenEvaluatorClientFactory
{
    public static TokenEvaluatorClient Create()
    {
        ModelEncodingProvider modelEncodingProvider = new();
        TokenizerProviderService tokenizerProviderService = new();

        // Create EncodingService directly instead of retrieving from a service provider
        IEncodingService encodingService = new EncodingService(modelEncodingProvider, tokenizerProviderService);

        return new TokenEvaluatorClient(encodingService);
    }
}