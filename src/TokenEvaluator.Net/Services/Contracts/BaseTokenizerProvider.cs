using System.Collections.Generic;
using TokenEvaluator.Net.Models;

namespace TokenEvaluator.Net.Services.Contracts
{
    public abstract class BaseTokenizerProvider : ITokenizerProvider
    {
        public IEmbeddedResourceQuery EmbeddedResourceQuery { get; internal set; }
        public BaseTokenizerProvider(IEmbeddedResourceQuery embeddedResourceQuery)
        {
            EmbeddedResourceQuery = embeddedResourceQuery;
        }

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