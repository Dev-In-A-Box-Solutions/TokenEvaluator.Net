using TokenEvaluator.Net.Services;
using TokenEvaluator.Net.Services.Contracts;

namespace TokenEvaluator.Net
{
    /// <summary>
    /// This is a concrete, tightly-coupled factory class that creates a TokenEvaluatorClient instance.
    /// </summary>
    public static class TokenEvaluatorClientFactory
    {
        public static TokenEvaluatorClient Create()
        {
            var embeddedResourceQuery = new EmbeddedResourceQuery();
            var modelEncodingProvider = new ModelEncodingProvider();
            var tokenizerProviderService = new TokenizerProviderService(embeddedResourceQuery);

            // Create EncodingService directly instead of retrieving from a service provider
            IEncodingService encodingService = new EncodingService(modelEncodingProvider, tokenizerProviderService);

            return new TokenEvaluatorClient(encodingService);
        }
    }
}