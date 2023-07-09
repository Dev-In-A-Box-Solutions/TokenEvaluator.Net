using System.Text;
using System.Text.RegularExpressions;
using TokenEvaluator.Net.Constants;
using TokenEvaluator.Net.Models;
using TokenEvaluator.Net.Services.Contracts;
using TokenEvaluator.Net.Tokenizers;

namespace TokenEvaluator.Net;

/// <summary>
/// Client class for tokenization encoding and decoding operations.
/// </summary>
public class TokenEvaluatorClient : ITokenEvaluatorClient
{
    private TokenizationEngine _tokenizationEngine;
    private TextTokenEncoding _textTokenEncoding;
    private string _pairedByteEncodingDirectory;

    private IEncodingService EncodingService { get; }

    public TokenEvaluatorClient(IEncodingService encodingService)
    {
        EncodingService = encodingService;

        // Set file directory.
        var _localAppDataDirectory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        _pairedByteEncodingDirectory = Path.Combine(_localAppDataDirectory, FileConstants.PAIRED_BYTE_ENCODING_DIRECTORY);
    }

    /// <summary>
    /// Sets the default token encoding for the specified model type.
    /// </summary>
    /// <param name="modelType">The type of the model to set the default encoding for.</param>
    public async Task SetDefaultTokenEncodingForModelAsync(ModelType modelType)
    {
        _textTokenEncoding = await EncodingService.GetEncodingFromModelAsync(modelType);
        _tokenizationEngine = new TokenizationEngine(_textTokenEncoding.MergeableRanks, _textTokenEncoding.SpecialTokens, _textTokenEncoding.PatternString);
    }

    /// <summary>
    /// Sets the default token encoding for the specified encoding type.
    /// </summary>
    /// <param name="encodingType">The type of encoding to set as default.</param>
    public async Task SetDefaultTokenEncodingAsync(EncodingType encodingType)
    {
        _textTokenEncoding = await EncodingService.GetEncodingAsync(encodingType);
        _tokenizationEngine = new TokenizationEngine(_textTokenEncoding.MergeableRanks, _textTokenEncoding.SpecialTokens, _textTokenEncoding.PatternString);
    }

    /// <summary>
    /// Decodes the specified list of token IDs into text.
    /// </summary>
    /// <param name="tokens">The list of token IDs.</param>
    /// <returns>The decoded text.</returns>
    public string Decode(List<int> tokens)
    {
        var ret = _tokenizationEngine.DecodeNative(tokens.ToArray());
        return Encoding.UTF8.GetString(ret.ToArray());
    }

    /// <summary>
    /// Encodes the specified text into a list of token IDs.
    /// </summary>
    /// <param name="text">The text to encode.</param>
    /// <param name="allowedSpecial">The allowed special tokens.</param>
    /// <param name="disallowedSpecial">The disallowed special tokens.</param>
    /// <returns>A list of token IDs representing the encoded text.</returns>
    public List<int> Encode(string text, object? allowedSpecial = null, object? disallowedSpecial = null)
    {
        var allowedSpecialSet = allowedSpecial switch
        {
            null => new HashSet<string>(),
            "all" => SpecialTokensSet(),
            _ => new HashSet<string>((IEnumerable<string>)allowedSpecial)
        };

        var disallowedSpecialSet = disallowedSpecial switch
        {
            null => new HashSet<string>(),
            "all" => new HashSet<string>(SpecialTokensSet().Except(allowedSpecialSet)),
            _ => new HashSet<string>((IEnumerable<string>)disallowedSpecial)
        };

        CheckDisallowedSpecial(text, disallowedSpecialSet);

        return _tokenizationEngine.EncodeNative(text, allowedSpecialSet).Item1;
    }

    /// <summary>
    /// Counts the number of encoded tokens in the given text.
    /// </summary>
    /// <param name="text">The text to encode.</param>
    /// <param name="allowedSpecial">The allowed special tokens.</param>
    /// <param name="disallowedSpecial">The disallowed special tokens.</param>
    /// <returns>The count of encoded tokens in the text.</returns>
    public int EncodedTokenCount(string text, object? allowedSpecial = null, object? disallowedSpecial = null)
    {
        return Encode(text, allowedSpecial, disallowedSpecial)?.Count ?? 0;
    }

    /// <summary>
    /// Gets the set of special tokens.
    /// </summary>
    /// <returns>A HashSet containing the special tokens.</returns>
    public HashSet<string> SpecialTokensSet()
    {
        try
        {
            return new HashSet<string>(_textTokenEncoding.SpecialTokens.Keys);
        }
        catch (Exception ex)
        {
            // Log the exception, you might have a logging mechanism
            // Console.WriteLine(ex);

            // You might want to handle different types of exceptions differently. 
            // For example, if _textTokenEncoding or SpecialTokens is null,
            // a NullReferenceException will be thrown.
            if (ex is NullReferenceException)
            {
                // handle NullReferenceException
                // Console.WriteLine("NullReferenceException caught: " + ex.Message);
            }

            // If no specific handling is available, you might want to rethrow the exception,
            // throw the general Exception, or return a default value.
            // Here's how you can return a default value:
            return new HashSet<string>();
        }
    }

    /// <summary>
    /// Sets the Pared Byte Encoding File Directory property of the EncodingManager instance. This overrides the defaults which are the Local Application Data Directory.
    /// </summary>
    public void OverridePairedByteEncodingDirectory(string directoryPath)
    {
        _pairedByteEncodingDirectory = directoryPath;
        EncodingService?.SetPairedByteEncodingDirectory(_pairedByteEncodingDirectory);
    }

    /// <summary>
    /// Checks if the text contains any disallowed special tokens and throws an exception if a match is found.
    /// </summary>
    /// <param name="text">The text to be checked.</param>
    /// <param name="disallowedSpecialSet">The set of disallowed special tokens.</param>
    private static void CheckDisallowedSpecial(string text, HashSet<string> disallowedSpecialSet)
    {
        if (disallowedSpecialSet.Count > 0)
        {
            var specialTokenRegex = SpecialTokenRegex(disallowedSpecialSet);
            var match = specialTokenRegex.Match(text);
            if (match.Success)
            {
                throw new Exception(match.Value);
            }
        }
    }

    /// <summary>
    /// Creates a regular expression pattern that matches any of the specified special tokens.
    /// </summary>
    /// <param name="tokens">The set of special tokens.</param>
    /// <returns>A regular expression pattern.</returns>
    private static Regex SpecialTokenRegex(HashSet<string> tokens)
    {
        var inner = string.Join("|", tokens.Select(Regex.Escape));
        return new Regex($"({inner})");
    }
}