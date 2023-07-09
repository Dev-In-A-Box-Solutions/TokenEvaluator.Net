namespace TokenEvaluator.Tests;

internal class Constants
{
    /// <summary>
    /// Tests the text tokenization with a variety of language features including:
    /// - Standard alphanumeric words (Example: "The", "quick", "brown", "fox", etc.)
    /// - Punctuation (Example: commas, dashes, period, apostrophe)
    /// - Special characters (Example: hyphen in "moonlit-night")
    /// - Numbers (both spelled out "ten" and numeral "10")
    /// - An address (Example: "123 Elm St.")
    /// - A time (Example: "7:30 PM")
    /// - A question (Example: "Isn't text tokenization interesting?")
    /// This variety should help test how the application handles different types of tokens in a sentence.
    /// </summary>
    internal const string GeneratedText = "The quick, brown fox—enamored by the moonlit night—jumped over 10 lazily sleeping dogs near 123 Elm St. at approximately 7:30 PM. Isn't text tokenization interesting?";
}