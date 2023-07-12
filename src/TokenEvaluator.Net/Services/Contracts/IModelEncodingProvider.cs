using TokenEvaluator.Net.Models;

namespace TokenEvaluator.Net.Services.Contracts
{
    public interface IModelEncodingProvider
    {
        public EncodingType GetEncodingForModel(ModelType modelType);
    }
}