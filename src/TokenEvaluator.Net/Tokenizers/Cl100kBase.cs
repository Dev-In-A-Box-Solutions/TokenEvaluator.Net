using TokenEvaluator.Net.Constants;
using TokenEvaluator.Net.Models;
using TokenEvaluator.Net.Services.Contracts;

namespace TokenEvaluator.Net.Tokenizers
{
    internal class Cl100kBase
    {
        internal BaseTokenizerProvider TokenizerProvider { get; set; }

        public Cl100kBase(BaseTokenizerProvider tokenizerProvider)
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
                Dictionary<byte[], int>? mergeableRanks = null;

                mergeableRanks = TokenizerProvider.LoadFromInternal(EncodingType.Cl100kBase);

                var specialTokens = new Dictionary<string, int>
                {
                    { TextMarkerConstants.ENDOFTEXT, 100257},
                    { TextMarkerConstants.FIM_PREFIX, 100258},
                    { TextMarkerConstants.FIM_MIDDLE, 100259},
                    { TextMarkerConstants.FIM_SUFFIX, 100260},
                    { TextMarkerConstants.ENDOFPROMPT, 100276}
                };

                return new TextTokenEncoding()
                {
                    Name = "cl100k_base",
                    PatternString = @"(?i:'s|'t|'re|'ve|'m|'ll|'d)|[^\r\n\p{L}\p{N}]?\p{L}+|\p{N}{1,3}| ?[^\s\p{L}\p{N}]+[\r\n]*|\s*[\r\n]+|\s+(?!\S)|\s+",
                    MergeableRanks = mergeableRanks ?? new Dictionary<byte[], int>(),
                    SpecialTokens = specialTokens
                };
            }
            return default;
        }
    }
}