using System.Collections.Generic;
using TokenEvaluator.Net.Models;

namespace TokenEvaluator.Net
{
    public interface ITokenEvaluatorClient
    {
        HashSet<string> SpecialTokensSet();
        List<int> Encode(string text, bool useParallelProcessing = true, object? allowedSpecial = null, object? disallowedSpecial = null);
        string Decode(List<int> tokens, bool useParallelProcessing = true);
        int EncodedTokenCount(string text, bool useUnsafe = true, bool useParallelProcessing = true, object? allowedSpecial = null, object? disallowedSpecial = null);
        void OverridePairedByteEncodingDirectory(string directoryPath);
        void SetDefaultTokenEncodingForModel(ModelType modelType);
        void SetDefaultTokenEncoding(EncodingType encodingType);
        double VisionTokenCount(int width, int height, DetailLevel detail);
    }
}