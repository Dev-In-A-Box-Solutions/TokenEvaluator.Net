using TextTokenizationEvaluator.Core.Models;
using TextTokenizationEvaluator.Core.Services.Contracts;

namespace TextTokenizationEvaluator.Core.Services;

internal class ModelEncodingProvider : IModelEncodingProvider
{
    /// <summary>
    /// Gets the corresponding encoding type for the given model type.
    /// </summary>
    /// <param name="modelType">The model type.</param>
    /// <returns>The encoding type.</returns>
    public EncodingType GetEncodingForModel(ModelType modelType)
    {
        return modelType switch
        {
            ModelType.Gpt4 or ModelType.Gpt3_5Turbo => EncodingType.Cl100kBase,
            ModelType.TextDavinci003 or ModelType.TextDavinci002 or ModelType.TextDavinci001 or ModelType.TextCurie001 or ModelType.TextBabbage001 or ModelType.TextAda001 or ModelType.Davinci or ModelType.Curie or ModelType.Babbage or ModelType.Ada or ModelType.CodeDavinci002 or ModelType.CodeDavinci001 or ModelType.CodeCushman002 or ModelType.CodeCushman001 or ModelType.DavinciCodex or ModelType.CushmanCodex or ModelType.TextDavinciEdit001 or ModelType.CodeDavinciEdit001 or ModelType.TextEmbeddingAda002 or ModelType.TextSimilarityDavinci001 or ModelType.TextSimilarityCurie001 or ModelType.TextSimilarityBabbage001 or ModelType.TextSimilarityAda001 or ModelType.TextSearchDavinciDoc001 or ModelType.TextSearchCurieDoc001 or ModelType.TextSearchBabbageDoc001 or ModelType.TextSearchAdaDoc001 or ModelType.CodeSearchBabbageCode001 or ModelType.CodeSearchAdaCode001 => EncodingType.P50kBase,
            _ => throw new NotImplementedException(),
        };
    }
}