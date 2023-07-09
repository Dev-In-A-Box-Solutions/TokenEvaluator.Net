using TextTokenizationEvaluator.Core.Models;

namespace TextTokenizationEvaluator.Core.Services.Contracts;

public interface ITokenizerProvider
{
    public Dictionary<byte[], int>? LoadFromInternal(EncodingType encodingType);
    public Task<Dictionary<byte[], int>>? LoadFromUrlOrCacheAsync(EncodingType encodingType, string cacheLocation = "");
}