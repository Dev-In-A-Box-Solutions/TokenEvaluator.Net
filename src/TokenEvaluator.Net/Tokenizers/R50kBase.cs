using System;
using System.Collections.Generic;
using TokenEvaluator.Net.Constants;
using TokenEvaluator.Net.Models;
using TokenEvaluator.Net.Services.Contracts;

namespace TokenEvaluator.Net.Tokenizers
{
    internal class R50kBase
    {
        internal BaseTokenizerProvider TokenizerProvider { get; set; }

        public R50kBase(BaseTokenizerProvider tokenizerProvider)
        {
            TokenizerProvider = tokenizerProvider ?? throw new ArgumentNullException(nameof(tokenizerProvider));
        }

        public void SetPairedByteEncodingDirectory(string directory)
        {
            TokenizerProvider?.SetPairedByteEncodingDirectory(directory);
        }

        internal TextTokenEncoding? GetEncoding()
        {
            if (TokenizerProvider != null)
            {
                Dictionary<byte[], int>? mergeableRanks = TokenizerProvider.LoadFromInternal(EncodingType.R50kBase);
                return new TextTokenEncoding()
                {
                    Name = "p50k_base",
                    ExplicitNVocab = 50257,
                    PatternString = @"'s|'t|'re|'ve|'m|'ll|'d| ?\p{L}+| ?\p{N}+| ?[^\s\p{L}\p{N}]+|\s+(?!\S)|\s+",
                    MergeableRanks = mergeableRanks ?? new Dictionary<byte[], int>(),
                    SpecialTokens = new Dictionary<string, int>
                    {
                        [TextMarkerConstants.ENDOFTEXT] = 50256,
                    }
            };
            }
            return default;
        }
    }
}