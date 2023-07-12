namespace TokenEvaluator.Net.Models
{
    public enum EncodingType
    {
        R50kBase,
        P50kBase,
        Cl100kBase
    }

    public enum ModelType
    {
        // Chat CL100K
        Gpt4,
        Gpt3_5Turbo,
        Gpt35Turbo,
        // p50_base/edit
        TextDavinci003,
        TextDavinci002,
        TextDavinci001,
        TextCurie001,
        TextBabbage001,
        TextAda001,
        Davinci,
        Curie,
        Babbage,
        Ada,
        // Code P50_base/edit
        CodeDavinci002,
        CodeDavinci001,
        CodeCushman002,
        CodeCushman001,
        DavinciCodex,
        CushmanCodex,
        // edit P50k_base/edit
        TextDavinciEdit001,
        CodeDavinciEdit001,
        // embeddings cl100k
        TextEmbeddingAda002,
        // older models R50k_base
        TextSimilarityDavinci001,
        TextSimilarityCurie001,
        TextSimilarityBabbage001,
        TextSimilarityAda001,
        TextSearchDavinciDoc001,
        TextSearchCurieDoc001,
        TextSearchBabbageDoc001,
        TextSearchAdaDoc001,
        CodeSearchBabbageCode001,
        CodeSearchAdaCode001
    }
}