﻿namespace TokenEvaluator.Tests;

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

    /// <summary>
    /// A long, consistent paragraph for performance testing of the tokenization algorithm. 
    /// This text is designed to stress the tokenizer with a variety of characters and 
    /// structures to simulate real-world usage.
    /// </summary>
    internal const string PerformanceTestString = "The swift brown fox leapt over the lazy dog, its bushy tail trailing behind it in the early morning light. Back in the den, a litter of cubs mewled, awaiting the return of their mother. She was out in the forest, hunting for the family. The cold of winter was fast approaching, and the family needed to store up as much food as they could. The father, meanwhile, was busy expanding the den, digging out more room for the growing family. He was a large, imposing figure with a thick coat of fur, brown with streaks of gray. His sharp eyes missed nothing, always alert for any sign of danger. Life in the forest was not easy, but it was their home and they wouldn't have it any other way. In the midst of the sprawling wilderness, their den was a bastion of warmth and safety. The forest teemed with life, with birds singing in the treetops, squirrels scampering up and down the trunks, and deer grazing in the clearings. There was a serene beauty to it, the sights and sounds forming a symphony of nature. The fox family was just one part of this grand tapestry, living out their lives day by day. As the sun began to set, painting the sky in hues of red and gold, the mother fox returned to the den. In her jaws, she carried a rabbit, its fur matted with the struggle of its final moments. The cubs pounced on it eagerly, their tiny teeth tearing into the flesh. As they ate, the mother fox cleaned her paws, watching them with a fond gaze. She knew that they would soon have to learn to hunt for themselves. But for now, they were her little cubs, and she would do all she could to protect and nourish them. As night fell, the family curled up together in the den, a tangle of fur and warmth. Outside, the forest fell quiet, with only the occasional hoot of an owl breaking the silence. The moon cast long shadows over the landscape, bathing everything in a soft, silvery glow. It was another day in the life of the fox family, filled with its own challenges and joys. And so it would go on, day after day, as the seasons changed and the cubs grew into adults. They were a part of the forest, and the forest was a part of them.\r\n";
}