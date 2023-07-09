using TokenEvaluator.Net.Models;

namespace TokenEvaluator.Net.Services.Contracts;

public interface ITokenizerProvider
{
    public Dictionary<byte[], int>? LoadFromInternal(EncodingType encodingType);
    public Task<Dictionary<byte[], int>>? LoadFromUrlOrCacheAsync(EncodingType encodingType);
}