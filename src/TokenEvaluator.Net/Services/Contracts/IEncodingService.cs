using TokenEvaluator.Net.Models;

namespace TokenEvaluator.Net.Services.Contracts;

public interface IEncodingService
{
    void SetPairedByteEncodingDirectory(string directory);
    Task<TextTokenEncoding> GetEncodingFromModelAsync(ModelType modelType);
    Task<TextTokenEncoding> GetEncodingAsync(EncodingType encodingType);
}