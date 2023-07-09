using TextTokenizationEvaluator.Core.Models;

namespace TextTokenizationEvaluator.Core.Services.Contracts
{
    public abstract class BaseTokenizerProvider : ITokenizerProvider
    {
        public string? PairedByteEncodingDirectory
        {
            get; private set;
        }

        public void SetPairedByteEncodingDirectory(string directory)
        {
            PairedByteEncodingDirectory = directory;
        }

        public abstract Dictionary<byte[], int>? LoadFromInternal(EncodingType encodingType);
        public abstract Task<Dictionary<byte[], int>>? LoadFromUrlOrCacheAsync(EncodingType encodingType, string cacheLocation = "");
    }
}