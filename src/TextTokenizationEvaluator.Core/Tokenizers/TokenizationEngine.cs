using System.Text;
using System.Text.RegularExpressions;
using TextTokenizationEvaluator.Core.EncodingUtils;

namespace TextTokenizationEvaluator.Core.Tokenizers
{
    /// <summary>
    /// This represents the tokenization engine based on data from a provided encoder.
    /// </summary>
    internal class TokenizationEngine
    {
        private readonly Dictionary<byte[], int> _encoder;
        private readonly Dictionary<string, int> _specialTokensEncoder;
        private readonly Regex _specialRegex;
        private readonly Regex _regex;
        private readonly Lazy<Dictionary<int, byte[]>> _lazyDecoder;
        private Dictionary<int, byte[]> Decoder => _lazyDecoder.Value;
        private readonly Dictionary<int, string> _specialTokensDecoder;

        /// <summary>
        /// Initializes a new instance of the TokenizationEngine class.
        /// </summary>
        /// <param name="encoder">The token encoder dictionary.</param>
        /// <param name="specialTokensEncoder">The special tokens encoder dictionary.</param>
        /// <param name="pattern">The pattern used for tokenization.</param>
        public TokenizationEngine(Dictionary<byte[], int> encoder, Dictionary<string, int> specialTokensEncoder, string pattern)
        {
            _encoder = encoder;
            _regex = new Regex(pattern, RegexOptions.Compiled);
            _specialRegex = new Regex(string.Join("|", specialTokensEncoder.Keys), RegexOptions.Compiled);
            _specialTokensEncoder = specialTokensEncoder;

            _lazyDecoder = new Lazy<Dictionary<int, byte[]>>(() =>
            {
                var decoder = new Dictionary<int, byte[]>();
                foreach (var kvp in _encoder)
                {
                    decoder[kvp.Value] = kvp.Key;
                }

                if (_encoder.Count != decoder.Count)
                {
                    throw new ArgumentException("Encoder and decoder sizes don't match");
                }

                return decoder;
            });

            _specialTokensDecoder = new Dictionary<int, string>();
            foreach (var kvp in specialTokensEncoder)
            {
                _specialTokensDecoder[kvp.Value] = kvp.Key;
            }
        }

        /// <summary>
        /// Encodes the provided text into a list of tokens.
        /// </summary>
        /// <param name="text">The text to encode.</param>
        /// <param name="allowedSpecial">The set of allowed special tokens.</param>
        /// <returns>A tuple containing the list of encoded tokens and the length of the last piece token.</returns>
        public (List<int>, int) EncodeNative(string text, HashSet<string> allowedSpecial)
        {
            var ret = new List<int>();
            int start = 0;
            int lastPieceTokenLen = 0;

            while (true)
            {
                Match nextSpecial;
                int startFind = start;
                while (true)
                {
                    nextSpecial = _specialRegex.Match(text, startFind);
                    if (!nextSpecial.Success)
                        break;
                    if (allowedSpecial.Contains(nextSpecial.Value))
                        break;
                    startFind = nextSpecial.Index + 1;
                }

                int end = nextSpecial.Success ? nextSpecial.Index : text.Length;

                foreach (Match mat in _regex.Matches(text.Substring(start, end - start)))
                {
                    var piece = Encoding.UTF8.GetBytes(mat.Value);
                    if (_encoder.TryGetValue(piece, out int token))
                    {
                        lastPieceTokenLen = 1;
                        ret.Add(token);
                        continue;
                    }

                    var tokens = BytePairEncoding.BytePairEncode(piece, _encoder);
                    lastPieceTokenLen = tokens.Count;
                    ret.AddRange(tokens);
                }

                if (nextSpecial.Success)
                {
                    var piece = nextSpecial.Value;
                    var token = _specialTokensEncoder[piece];
                    ret.Add(token);
                    start = nextSpecial.Index + nextSpecial.Length;
                    lastPieceTokenLen = 0;
                }
                else
                {
                    break;
                }
            }
            return (ret, lastPieceTokenLen);
        }

        /// <summary>
        /// Decodes the provided tokens into a byte array.
        /// </summary>
        /// <param name="tokens">The tokens to decode.</param>
        /// <returns>The decoded byte array.</returns>
        public byte[] DecodeNative(int[] tokens)
        {
            var ret = new List<byte>(tokens.Length * 2);
            foreach (var token in tokens)
            {
                byte[] tokenBytes;
                if (Decoder.TryGetValue(token, out var value))
                {
                    tokenBytes = value;
                }
                else if (_specialTokensDecoder.TryGetValue(token, out var valueS))
                {
                    tokenBytes = Encoding.UTF8.GetBytes(valueS);
                }
                else
                {
                    continue;
                }
                ret.AddRange(tokenBytes);
            }
            return ret.ToArray();
        }
    }
}