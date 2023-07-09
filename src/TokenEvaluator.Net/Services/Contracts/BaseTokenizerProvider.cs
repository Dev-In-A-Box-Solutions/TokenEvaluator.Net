using TokenEvaluator.Net.Models;

namespace TokenEvaluator.Net.Services.Contracts
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
    }
}