namespace TokenEvaluator.Net.Constants
{
    internal class TextMarkerConstants
    {
        internal const string ENDOFTEXT = "<|endoftext|>";
        internal const string FIM_PREFIX = "<|fim_prefix|>";
        internal const string FIM_MIDDLE = "<|fim_middle|>";
        internal const string FIM_SUFFIX = "<|fim_suffix|>";
        internal const string ENDOFPROMPT = "<|endofprompt|>";
    }

    internal class RoBERTaTextMarkerConstants
    {
        internal const string ENDOFTEXT = "<s>";
        internal const string FIM_PREFIX = "<pad>";
        internal const string FIM_MIDDLE = "</s>";
        internal const string FIM_SUFFIX = "<unk>";
        internal const string ENDOFPROMPT = ",";
    }

    internal class FileConstants
    {
        internal const string PAIRED_BYTE_ENCODING_DIRECTORY = "PairedByteEncodingDirectory";
        internal const string PAIRED_BYTE_ENCODING_URLCACHE_DIRECTORY = "PairedByteEncodingUrlCachedDirectory";
    }
}