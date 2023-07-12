using System.Collections.Generic;
using TokenEvaluator.Net.Models;

namespace TokenEvaluator.Net.Services.Contracts
{
    public interface ITokenizerProvider
    {
        public Dictionary<byte[], int>? LoadFromInternal(EncodingType encodingType);
    }
}