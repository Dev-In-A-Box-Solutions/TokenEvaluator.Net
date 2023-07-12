using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TokenEvaluator.Net.EncodingUtils;
using TokenEvaluator.Net.Models;
using TokenEvaluator.Net.Services.Contracts;

namespace TokenEvaluator.Net.Services
{
    internal class TokenizerProviderService : BaseTokenizerProvider
    {
        public TokenizerProviderService(IEmbeddedResourceQuery embeddedResourceQuery) : base(embeddedResourceQuery)
        {
        }

        public override Dictionary<byte[], int>? LoadFromInternal(EncodingType encodingType)
        {
            var directory = "Encodings";
            var fullPath = string.Empty;

            switch (encodingType)
            {
                case EncodingType.Cl100kBase:
                    fullPath = Path.Combine(directory, "tiktoken", "cl100k_base.tiktoken");
                    break;
                case EncodingType.R50kBase:
                    fullPath = Path.Combine(directory, "tiktoken", "r50k_base.tiktoken");
                    break;
                case EncodingType.P50kBase:
                    fullPath = Path.Combine(directory, "tiktoken", "p50k_base.tiktoken");
                    break;
            }

            if (!string.IsNullOrEmpty(fullPath))
            {
                using var stream = EmbeddedResourceQuery.Read<TokenizerProviderService>(fullPath) ?? throw new FileNotFoundException($"The file {fullPath} does not exist.");
                using var reader = new StreamReader(stream);
                switch (encodingType)
                {
                    case EncodingType.Cl100kBase:
                    case EncodingType.R50kBase:
                    case EncodingType.P50kBase:
#if NETSTANDARD2_1_OR_GREATER || NET6_0_OR_GREATER
                        var contents = reader.ReadToEnd().Split(Environment.NewLine);
#else
                        var contents = reader.ReadToEnd().Split(new[] { Environment.NewLine }, StringSplitOptions.None);
#endif
                        var pairedByteEncodingDict = new Dictionary<byte[], int>(contents.Length, new ByteArrayComparer());
                        foreach (var line in contents.Where(l => !string.IsNullOrWhiteSpace(l)))
                        {
                            var tokens = line.Split();
                            var tokenBytes = Convert.FromBase64String(tokens[0]);
                            var rank = int.Parse(tokens[1]);
                            pairedByteEncodingDict.Add(tokenBytes, rank);
                        }
                        return pairedByteEncodingDict;
                }
            }
            return default;
        }
    }
}