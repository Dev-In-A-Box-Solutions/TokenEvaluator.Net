using System;
using TokenEvaluator.Net.Models;
using TokenEvaluator.Net.Services.Contracts;

namespace TokenEvaluator.Net.Services
{
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
                ModelType.Gpt4 or ModelType.Gpt3_5Turbo or ModelType.Gpt35Turbo => EncodingType.Cl100kBase,
                ModelType.TextDavinci001 or ModelType.TextDavinci002 or ModelType.TextDavinci003 or ModelType.TextCurie001 or ModelType.TextBabbage001 or ModelType.TextAda001 or ModelType.Davinci or ModelType.Curie or ModelType.Babbage or ModelType.Ada => EncodingType.P50kBase,
                ModelType.CodeDavinci001 or ModelType.CodeDavinci002 or ModelType.CodeCushman001 or ModelType.CodeCushman002 or ModelType.DavinciCodex or ModelType.CushmanCodex => EncodingType.P50kBase,
                ModelType.TextDavinciEdit001 or ModelType.CodeDavinciEdit001 => EncodingType.P50kBase,
                ModelType.TextEmbeddingAda002 => EncodingType.Cl100kBase,
                ModelType.TextSimilarityDavinci001 or ModelType.TextSimilarityCurie001 or ModelType.TextSimilarityBabbage001 or ModelType.TextSimilarityAda001 or ModelType.TextSearchDavinciDoc001 or ModelType.TextSearchCurieDoc001 or ModelType.TextSearchBabbageDoc001 or ModelType.TextSearchAdaDoc001 or ModelType.CodeSearchBabbageCode001 or ModelType.CodeSearchAdaCode001 => EncodingType.R50kBase,
                _ => throw new NotImplementedException(),
            };
        }
    }
}