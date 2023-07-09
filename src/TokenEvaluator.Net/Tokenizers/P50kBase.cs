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

        internal async Task<TextTokenEncoding?> GetEncodingAsync(bool useAssemblyCachedFile = false)
        {
            if (TokenizerProvider != null)
            {
                Dictionary<byte[], int>? mergeableRanks;
                if (useAssemblyCachedFile)
                {
                    mergeableRanks = TokenizerProvider.LoadFromInternal(EncodingType.P50kBase);
                }
                else
                {
                    mergeableRanks = await TokenizerProvider.LoadFromUrlOrCacheAsync(EncodingType.P50kBase);
                }

                var specialTokens = new Dictionary<string, int>
                {
                    {TextMarkerConstants.ENDOFTEXT, 50256}
                };

                return new TextTokenEncoding()
                {
                    Name = "p50k_base",
                    ExplicitNVocab = 50281,
                    PatternString = @"'s|'t|'re|'ve|'m|'ll|'d| ?\p{L}+| ?\p{N}+| ?[^\s\p{L}\p{N}]+|\s+(?!\S)|\s+",
                    MergeableRanks = mergeableRanks ?? new Dictionary<byte[], int>(),
                    SpecialTokens = specialTokens
                };
            }
            return default;
        }
    }
}