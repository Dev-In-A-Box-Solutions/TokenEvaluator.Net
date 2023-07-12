using TokenEvaluator.Net.Models;

namespace TokenEvaluator.Net.Services.Contracts
{
    public interface IEncodingService
    {
        void SetPairedByteEncodingDirectory(string directory);
        TextTokenEncoding GetEncodingFromModel(ModelType modelType);
        TextTokenEncoding GetEncoding(EncodingType encodingType);
    }
}