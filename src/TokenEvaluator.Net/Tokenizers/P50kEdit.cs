using System;
using System.Collections.Generic;
using TokenEvaluator.Net.Constants;
using TokenEvaluator.Net.Models;
using TokenEvaluator.Net.Services.Contracts;

namespace TokenEvaluator.Net.Tokenizers
{
    internal class P50kBase
    {
        internal BaseTokenizerProvider TokenizerProvider { get; set; }

        public P50kBase(BaseTokenizerProvider tokenizerProvider)
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
                Dictionary<byte[], int>? mergeableRanks = TokenizerProvider.LoadFromInternal(EncodingType.P50kBase);
                return new TextTokenEncoding()
                {
                    Name = "p50k_edit",
                    ExplicitNVocab = 50281,
                    PatternString = @"'s|'t|'re|'ve|'m|'ll|'d| ?\p{L}+| ?\p{N}+| ?[^\s\p{L}\p{N}]+|\s+(?!\S)|\s+",
                    MergeableRanks = mergeableRanks ?? new Dictionary<byte[], int>(),
                    SpecialTokens = new Dictionary<string, int>
                    {
                        { TextMarkerConstants.ENDOFTEXT, 50256},
                        { TextMarkerConstants.FIM_PREFIX, 50281},
                        { TextMarkerConstants.FIM_MIDDLE, 50282},
                        { TextMarkerConstants.FIM_SUFFIX, 50283},
                    },
                };
            }
            return default;
        }
    }
}
