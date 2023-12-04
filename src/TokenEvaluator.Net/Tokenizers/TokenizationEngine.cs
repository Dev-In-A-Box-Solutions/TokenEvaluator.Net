using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using TokenEvaluator.Net.EncodingUtils;

namespace TokenEvaluator.Net.Tokenizers
{
    /// <summary>
    /// This represents the tokenization engine based on data from a provided encoder.
    /// </summary>
    internal class TokenizationEngine
    {
        private readonly IReadOnlyDictionary<byte[], int> _encoder;
        private readonly IReadOnlyDictionary<string, int> _specialTokensEncoder;
        private readonly Regex _specialRegex;
        private readonly Regex _regex;
        private readonly Lazy<IReadOnlyDictionary<int, byte[]>> _lazyDecoder;
        private IReadOnlyDictionary<int, byte[]> Decoder => _lazyDecoder.Value;
        private readonly IReadOnlyDictionary<int, string> _specialTokensDecoder;
        private IReadOnlyDictionary<string, int> FastEncoder { get; set; }
        private IDictionary<string, IReadOnlyCollection<int>> Cache { get; set; } = new ConcurrentDictionary<string, IReadOnlyCollection<int>>();
        private IDictionary<string, int> CachedCounts { get; set; } = new ConcurrentDictionary<string, int>();

        internal bool EnableCache { get; set; } = true;

        /// <summary>
        /// Initializes a new instance of the TokenizationEngine class.
        /// </summary>
        /// <param name="encoder">The token encoder dictionary.</param>
        /// <param name="specialTokensEncoder">The special tokens encoder dictionary.</param>
        /// <param name="pattern">The pattern used for tokenization.</param>
        public TokenizationEngine(Dictionary<byte[], int> encoder, Dictionary<string, int> specialTokensEncoder, string pattern)
        {
            _encoder = encoder;
            FastEncoder = _encoder
            .ToDictionary(
                static x => new string(x.Key.Select(y => (char)y).ToArray()),
                static x => x.Value);

            _regex = new Regex(pattern, RegexOptions.Compiled);
            _specialRegex = new Regex(string.Join("|", specialTokensEncoder.Keys), RegexOptions.Compiled);
            _specialTokensEncoder = specialTokensEncoder;

            _lazyDecoder = new Lazy<IReadOnlyDictionary<int, byte[]>>(() =>
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

            _specialTokensDecoder = specialTokensEncoder.ToDictionary(kvp => kvp.Value, kvp => kvp.Key);
        }

        /// <summary>
        /// Counts the number of tokenized units in a given string using native system calls and parallel processing.
        /// </summary>
        /// <remarks>
        /// This method tokenizes the input string using the regex matches.
        /// It can use parallel processing to increase speed when the useParallelProcessing parameter is set to true. 
        /// For each match, it first checks a fast encoder dictionary and a cache (if enabled) to find the token count.
        /// If the token count is not found in the dictionary or cache, it calculates the count using byte pair encoding (BPE).
        /// It also updates the cache with the BPE token count for future use if caching is enabled.
        /// </remarks>
        /// <param name="text">The input string to count tokenized units.</param>
        /// <param name="useParallelProcessing">Indicates whether to use parallel processing for increased speed.</param>
        /// <returns>An integer representing the count of tokenized units in the input string.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the input text is null.</exception>
        public int CountTokensNativeUnsafe(string text, bool useParallelProcessing = true)
        {
            text = text ?? throw new ArgumentNullException(nameof(text));
            int tokens = 0;

            if (useParallelProcessing)
            {
                var matches = _regex.Matches(text).Cast<Match>();

                Parallel.ForEach(matches, match =>
                {
                    var matchValue = match.Value;
                    var fastKey = matchValue;
                    var piece = Encoding.UTF8.GetBytes(matchValue);
                    int numberOfTokens = 0;

                    if (FastEncoder.ContainsKey(fastKey))
                    {
                        numberOfTokens++;
                    }
                    else if (EnableCache && CachedCounts.TryGetValue(fastKey, out int toks))
                    {
                        numberOfTokens = toks;
                    }
                    else if (_encoder.ContainsKey(piece))
                    {
                        numberOfTokens++;
                    }
                    else
                    {
                        numberOfTokens = BytePairEncoding.BytePairEncodeCountTokens(piece, _encoder);

                        if (EnableCache)
                        {
                            CachedCounts[fastKey] = numberOfTokens;
                        }
                    }
                    Interlocked.Add(ref tokens, numberOfTokens);
                });
            }
            else
            {
#if NET7_0_OR_GREATER
                var textSpan = text.AsSpan();
                var matches = _regex.EnumerateMatches(textSpan);
#else
                var matches = _regex.Matches(text).Cast<Match>();
#endif
                foreach (var match in matches)
                {
#if NET7_0_OR_GREATER
                    var matchValue = textSpan.Slice(match.Index, match.Length).ToArray();
                    var fastKey = new string(textSpan.Slice(match.Index, match.Length));
#else
                    var matchValue = match.Value;
                    var fastKey = matchValue;
#endif
                    if (FastEncoder.ContainsKey(fastKey))
                    {
                        tokens++;
                        continue;
                    }
                    if (EnableCache && CachedCounts.TryGetValue(fastKey, out var fastNumberOfTokens))
                    {
                        tokens += fastNumberOfTokens;
                        continue;
                    }

                    var piece = System.Text.Encoding.UTF8.GetBytes(matchValue);
                    if (_encoder.ContainsKey(piece))
                    {
                        tokens++;
                        continue;
                    }

                    var numberOfTokens = BytePairEncoding.BytePairEncodeCountTokens(piece, _encoder);
                    tokens += numberOfTokens;

                    if (EnableCache)
                    {
                        CachedCounts[fastKey] = numberOfTokens;
                    }
                }
            }
            return tokens;
        }

        /// <summary>
        /// Counts the total number of tokens in the given text using the Byte Pair Encoding (BPE) scheme.
        /// The method scans the text for special tokens, regular tokens, and falls back to encoding sequences
        /// of bytes when no match is found in the encoder dictionary. Special tokens are also checked against 
        /// an allowed list.
        /// </summary>
        /// <param name="text">The input text to tokenize and count.</param>
        /// <param name="allowedSpecial">A set of special tokens allowed in the tokenization process.</param>
        /// <returns>An integer representing the total count of tokens.</returns>
        public int CountTokensNative(string text, HashSet<string> allowedSpecial, bool useParallelProcessing = true)
        {
            if (string.IsNullOrEmpty(text))
            {
                return 0;
            }

            int tokenCount = 0;
            int start = 0;

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

                if (useParallelProcessing)
                {
#if NETSTANDARD2_1_OR_GREATER || NET6_0_OR_GREATER
                    var matches = _regex.Matches(text[start..end]);
#else
                    var matches = _regex.Matches(text.Substring(start, end - start)).Cast<Match>();
#endif
                    var syncLock = new object();

                    Parallel.ForEach(matches, mat =>
                    {
                        var piece = Encoding.UTF8.GetBytes(mat.Value);
                        if (_encoder.TryGetValue(piece, out int _))
                        {
                            lock (syncLock)
                            {
                                tokenCount++;
                            }
                            return;
                        }

                        var tokens = BytePairEncoding.BytePairEncode(piece, _encoder);
                        lock (syncLock)
                        {
                            tokenCount += tokens.Count;
                        }
                    });

                }
                else
                {
#if NETSTANDARD2_1_OR_GREATER || NET6_0_OR_GREATER
                    foreach (Match mat in _regex.Matches(text[start..end]))
#else
                    foreach (Match mat in _regex.Matches(text.Substring(start, end - start)))
#endif
                    {
                        var piece = Encoding.UTF8.GetBytes(mat.Value);
                        if (_encoder.TryGetValue(piece, out int _))
                        {
                            tokenCount++;
                            continue;
                        }

                        var tokens = BytePairEncoding.BytePairEncode(piece, _encoder);
                        tokenCount += tokens.Count;
                    }
                }

                if (nextSpecial.Success)
                {
                    start = nextSpecial.Index + nextSpecial.Length;
                    tokenCount++;
                }
                else
                {
                    break;
                }
            }
            return tokenCount;
        }

        /// <summary>
        /// Encodes the provided text into a list of tokens.
        /// </summary>
        /// <param name="text">The text to encode.</param>
        /// <param name="allowedSpecial">The set of allowed special tokens.</param>
        /// <returns>A tuple containing the list of encoded tokens and the length of the last piece token.</returns>
        public (List<int>, int) ParallelEncodeNative(string text, HashSet<string> allowedSpecial)
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

#if NETSTANDARD2_1_OR_GREATER || NET6_0_OR_GREATER
                var matches = _regex.Matches(text[start..end]).Cast<Match>();
#else
                var matches = _regex.Matches(text.Substring(start, end - start)).Cast<Match>();
#endif
                var concurrentTokensList = new ConcurrentBag<List<int>>();
                Parallel.ForEach(matches, mat =>
                {
                    var piece = Encoding.UTF8.GetBytes(mat.Value);
                    if (_encoder.TryGetValue(piece, out int token))
                    {
                        concurrentTokensList.Add(new List<int> { token });
                    }
                    else
                    {
                        var tokens = BytePairEncoding.BytePairEncode(piece, _encoder);
                        concurrentTokensList.Add(tokens);
                    }
                });
                ret.AddRange(concurrentTokensList.SelectMany(x => x));

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
        /// Decodes the provided tokens into a byte array using optional parallel processing.
        /// </summary>
        /// <param name="tokens">The tokens to decode.</param>
        /// <param name="useParallelProcessing">If true, decodes tokens in parallel.</param>
        /// <returns>The decoded byte array.</returns>
        public byte[] ParallelDecodeNative(int[] tokens)
        {
            var retParallel = new ConcurrentBag<byte>();
            Parallel.ForEach(tokens, token =>
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
                    return;
                }

                foreach (var b in tokenBytes)
                {
                    retParallel.Add(b);
                }
            });
            return retParallel.ToArray();
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