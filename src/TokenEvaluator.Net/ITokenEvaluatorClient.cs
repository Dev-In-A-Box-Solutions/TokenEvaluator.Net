using TokenEvaluator.Net.Models;

namespace TokenEvaluator.Net
{
    public interface ITokenEvaluatorClient
    {
        HashSet<string> SpecialTokensSet();
        List<int> Encode(string text, object? allowedSpecial = null, object? disallowedSpecial = null);
        string Decode(List<int> tokens);
        int EncodedTokenCount(string text, object? allowedSpecial = null, object? disallowedSpecial = null);
        void OverridePairedByteEncodingDirectory(string directoryPath);
        Task SetDefaultTokenEncodingForModelAsync(ModelType modelType);
        Task SetDefaultTokenEncodingAsync(EncodingType encodingType);
    }
}