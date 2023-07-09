using TokenEvaluator.Net.Constants;
using TokenEvaluator.Net.Models;
using TokenEvaluator.Net.Services.Contracts;
using TokenEvaluator.Net.Tokenizers;

namespace TokenEvaluator.Net.Services;

public class EncodingService : IEncodingService
{
    private readonly IModelEncodingProvider _modelEncodingProvider;
    private readonly BaseTokenizerProvider _tokenizerProvider;
    private Cl100kBase _cl100k_base;
    private P50kBase _p50KBase;

    public EncodingService(IModelEncodingProvider modelEncodingProvider, BaseTokenizerProvider tokenizerProvider)
    {
        _modelEncodingProvider = modelEncodingProvider;
        _tokenizerProvider = tokenizerProvider;

        // get directory for cached data
        var _localAppDataDirectory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        var directory = Path.Combine(_localAppDataDirectory, FileConstants.PAIRED_BYTE_ENCODING_URLCACHE_DIRECTORY);

        // set default directory for paired byte encoding files
        _tokenizerProvider.SetPairedByteEncodingDirectory(directory);

        // initialise our available tiktoken base classes
        _cl100k_base = new Cl100kBase(tokenizerProvider);
        _p50KBase = new P50kBase(tokenizerProvider);
    }

    public void SetPairedByteEncodingDirectory(string directory)
    {
        // set default directory for paired byte encoding files
        _tokenizerProvider.SetPairedByteEncodingDirectory(directory);

        // re-initialise our available tiktoken base classes
        _cl100k_base = new Cl100kBase(_tokenizerProvider);
        _p50KBase = new P50kBase(_tokenizerProvider);
    }

    public TextTokenEncoding GetEncodingFromModel(ModelType modelType) => GetEncoding(_modelEncodingProvider.GetEncodingForModel(modelType));

    public TextTokenEncoding GetEncoding(EncodingType encodingType)
    {
        switch (encodingType)
        {
            case EncodingType.P50kBase:
                {
                    return _p50KBase.GetEncoding();
                }
            case EncodingType.Cl100kBase:
                {
                    return _cl100k_base.GetEncoding();
                }
            default:
                throw new NotImplementedException();
        }
    }
}