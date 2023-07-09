using System.Text;
using TokenEvaluator.Net.Constants;
using TokenEvaluator.Net.EncodingUtils;
using TokenEvaluator.Net.Models;
using TokenEvaluator.Net.Services.Contracts;

namespace TokenEvaluator.Net.Services;

internal class TokenizerProviderService : BaseTokenizerProvider
{
    private static readonly HttpClient _httpClient = new();

    /// <summary>
    /// Loads the given encoding type from the libraries local content directory.
    /// </summary>
    /// <param name="encodingType"></param>
    /// <returns></returns>
    public override Dictionary<byte[], int>? LoadFromInternal(EncodingType encodingType)
    {
        var _localAppDataDirectory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        var directory = Path.Combine(_localAppDataDirectory, FileConstants.PAIRED_BYTE_ENCODING_DIRECTORY);
        var fullPath = string.Empty;

        switch (encodingType)
        {
            case EncodingType.Cl100kBase:
                fullPath = Path.Combine(directory, "cl100k_base.tiktoken");
                break;
            case EncodingType.P50kBase:
                fullPath = Path.Combine(directory, "p50k_base.tiktoken");
                break;
        }

        if (!string.IsNullOrEmpty(fullPath))
        {
            var contents = File.ReadAllLines(fullPath, Encoding.UTF8);
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
        return default;
    }

    /// <summary>
    /// Attempts to load the given encoding type from the downloaded cache store or if first run the corresponding provider URL; if both fail this falls back to the local content directory version of the encoding token file.
    /// </summary>
    /// <param name="bytePairEncodingFileUrl"></param>
    /// <returns></returns>
    /// <exception cref="FormatException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public override async Task<Dictionary<byte[], int>>? LoadFromUrlOrCacheAsync(EncodingType encodingType, string? cacheLocation)
    {
        var directory = string.IsNullOrEmpty(cacheLocation) ?
            PairedByteEncodingDirectory :
            cacheLocation;

        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        string url = encodingType switch
        {
            EncodingType.P50kBase => UrlConstants.PAIRED_BYTE_ENCODING_URL_P50K,
            EncodingType.Cl100kBase => UrlConstants.PAIRED_BYTE_ENCODING_URL_CL100K,
            _ => throw new ArgumentException($"Unsupported encoding type: {encodingType}"),
        };
        var fileName = Path.GetFileName(url);
        var localFilePath = Path.Combine(directory, fileName);

        if (!File.Exists(localFilePath))
        {
            try
            {
                var data = await _httpClient.GetByteArrayAsync(url);
                await File.WriteAllBytesAsync(localFilePath, data);
            }
            catch (HttpRequestException e)
            {
                throw new InvalidOperationException($"Failed to download encoding file from {url}: {e.Message}", e);
            }
        }

        var pairedByteEncoderDict = new Dictionary<byte[], int>(new ByteArrayComparer());

        try
        {
            var lines = await File.ReadAllLinesAsync(localFilePath, Encoding.UTF8);
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                var tokens = line.Split(' ');
                if (tokens.Length != 2)
                {
                    throw new FormatException($"Invalid file format: {localFilePath}");
                }

                var tokenBytes = Convert.FromBase64String(tokens[0]);
                var rank = int.Parse(tokens[1]);
                pairedByteEncoderDict[tokenBytes] = rank;
            }
        }
        catch (IOException e)
        {
            throw new InvalidOperationException($"Failed to read encoding file from {localFilePath}: {e.Message}", e);
        }
        catch (FormatException e)
        {
            throw new InvalidOperationException($"Invalid encoding file format in {localFilePath}: {e.Message}", e);
        }

        return pairedByteEncoderDict;
    }
}