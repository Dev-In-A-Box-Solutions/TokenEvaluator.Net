using TokenEvaluator.Net.Models;

namespace TokenEvaluator.Net.Services.Contracts;

public interface IEncodingService
{
    void SetPairedByteEncodingDirectory(string directory);
    Task<TextTokenEncoding> GetEncodingFromModelAsync(ModelType modelType);
    TextTokenEncoding GetEncodingFromModel(ModelType modelType);
    Task<TextTokenEncoding> GetEncodingAsync(EncodingType encodingType);
    TextTokenEncoding GetEncoding(EncodingType encodingType);
}