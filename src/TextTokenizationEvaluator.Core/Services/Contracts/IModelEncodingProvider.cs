using TextTokenizationEvaluator.Core.Models;

namespace TextTokenizationEvaluator.Core.Services.Contracts;

public interface IModelEncodingProvider
{
   public EncodingType GetEncodingForModel(ModelType modelType);
}