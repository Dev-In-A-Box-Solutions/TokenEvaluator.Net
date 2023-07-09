namespace TextTokenizationEvaluator.Core.Constants;

internal class TextMarkerConstants
{
    internal const string ENDOFTEXT = "<|endoftext|>";
    internal const string FIM_PREFIX = "<|fim_prefix|>";
    internal const string FIM_MIDDLE = "<|fim_middle|>";
    internal const string FIM_SUFFIX = "<|fim_suffix|>";
    internal const string ENDOFPROMPT = "<|endofprompt|>";
}

internal class FileConstants
{
    internal const string PAIRED_BYTE_ENCODING_DIRECTORY = "PairedByteEncodingDirectory";
    internal const string PAIRED_BYTE_ENCODING_URLCACHE_DIRECTORY = "PairedByteEncodingUrlCachedDirectory";
}

internal class UrlConstants
{
    internal const string PAIRED_BYTE_ENCODING_URL_P50K = "https://openaipublic.blob.core.windows.net/encodings/p50k_base.tiktoken";
    internal const string PAIRED_BYTE_ENCODING_URL_CL100K = "https://openaipublic.blob.core.windows.net/encodings/cl100k_base.tiktoken";
}