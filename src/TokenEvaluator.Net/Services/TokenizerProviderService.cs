using System.Reflection;
using System.Text;
using TokenEvaluator.Net.Constants;
using TokenEvaluator.Net.EncodingUtils;
using TokenEvaluator.Net.Models;
using TokenEvaluator.Net.Services.Contracts;

namespace TokenEvaluator.Net.Services;

internal class TokenizerProviderService : BaseTokenizerProvider
{
    public override Dictionary<byte[], int>? LoadFromInternal(EncodingType encodingType)
    {
        var directory = "Assets";
        var fullPath = string.Empty;

        switch (encodingType)
        {
            case EncodingType.Cl100kBase:
                fullPath = Path.Combine(directory, "TikToken", "cl100k_base.tiktoken");
                break;
            case EncodingType.P50kBase:
                fullPath = Path.Combine(directory, "TikToken", "p50k_base.tiktoken");
                break;
        }

        if (!string.IsNullOrEmpty(fullPath))
        {
            // Since we're working with file paths relative to the output directory,
            // we need to resolve them to absolute paths based on the current assembly location.
            var assemblyLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var absolutePath = Path.Combine(assemblyLocation, fullPath);

            if (!File.Exists(absolutePath))
            {
                throw new FileNotFoundException($"The file {absolutePath} does not exist.");
            }

            var contents = File.ReadAllLines(absolutePath, Encoding.UTF8);
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
    /// Loads the given encoding type from the libraries local content directory.
    /// </summary>
    /// <param name="encodingType"></param>
    /// <returns></returns>
    //public override Dictionary<byte[], int>? LoadFromInternal(EncodingType encodingType)
    //{
    //    var directory = "assets/tiktoken";
    //    var fullPath = string.Empty;

    //    switch (encodingType)
    //    {
    //        case EncodingType.Cl100kBase:
    //            fullPath = Path.Combine(directory, "cl100k_base.tiktoken");
    //            break;
    //        case EncodingType.P50kBase:
    //            fullPath = Path.Combine(directory, "p50k_base.tiktoken");
    //            break;
    //    }

    //    var assemblyLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
    //    var absolutePath = Path.Combine(assemblyLocation, fullPath);

    //    if (!string.IsNullOrEmpty(fullPath))
    //    {
    //        var contents = File.ReadAllLines(fullPath, Encoding.UTF8);
    //        var pairedByteEncodingDict = new Dictionary<byte[], int>(contents.Length, new ByteArrayComparer());

    //        foreach (var line in contents.Where(l => !string.IsNullOrWhiteSpace(l)))
    //        {
    //            var tokens = line.Split();
    //            var tokenBytes = Convert.FromBase64String(tokens[0]);
    //            var rank = int.Parse(tokens[1]);
    //            pairedByteEncodingDict.Add(tokenBytes, rank);
    //        }
    //        return pairedByteEncodingDict;
    //    }
    //    return default;
    //}
}