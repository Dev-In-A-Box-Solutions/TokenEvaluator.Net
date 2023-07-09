namespace TextTokenizationEvaluator.Core.Models;

public class TextTokenEncoding
{
    /// <summary>
    /// Gets or sets the explicit number of vocabulary words, if any.
    /// </summary>
    public int? ExplicitNVocab { get; set; }

    /// <summary>
    /// Gets the maximum token value from either MergeableRanks or SpecialTokens.
    /// </summary>
    public int MaxTokenValue
    {
        get
        {
            return Math.Max(MergeableRanks.Values.Max(), SpecialTokens.Values.Max());
        }
    }

    /// <summary>
    /// Gets or sets the dictionary of mergeable ranks. 
    /// The keys are byte arrays representing tokens, and the values are their corresponding ranks.
    /// </summary>
    public Dictionary<byte[], int> MergeableRanks { get; set; }

    /// <summary>
    /// Gets or sets the name of the TextTokenEncoding.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the pattern string used for encoding.
    /// </summary>
    public string PatternString { get; set; }

    /// <summary>
    /// Gets or sets the dictionary of special tokens. 
    /// The keys are strings representing special tokens, and the values are their corresponding ranks.
    /// </summary>
    public Dictionary<string, int> SpecialTokens { get; set; }

    /// <summary>
    /// Initializes a new instance of the TextTokenEncoding class.
    /// </summary>
    public TextTokenEncoding() { }
}