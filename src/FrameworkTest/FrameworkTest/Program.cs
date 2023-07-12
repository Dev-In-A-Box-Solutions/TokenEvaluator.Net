using TokenEvaluator.Net;
using TokenEvaluator.Net.Models;

namespace FrameworkTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var client = TokenEvaluatorClientFactory.Create();
            client.SetDefaultTokenEncoding(EncodingType.R50kBase);
            var tokenCount = client.EncodedTokenCount(Constants.GeneratedText);
        }
    }
}