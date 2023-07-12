using Benchmark.ConsoleApp.Constants;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using Microsoft.Extensions.DependencyInjection;
using SharpToken;
using TiktokenSharp;
using TokenEvaluator.Net;
using TokenEvaluator.Net.Dependency;
using TokenEvaluator.Net.Models;

namespace Benchmark.ConsoleApp
{
    [MemoryDiagnoser]
    [MarkdownExporter, RPlotExporter]
    [RankColumn, AllStatisticsColumn]
    [MarkdownExporterAttribute.GitHub]
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    [CategoriesColumn]
    [HideColumns("Error", "StdDev", "StdDev", "RatioSD")]
    public class Benchmarks
    {
        internal readonly IServiceCollection services = new ServiceCollection();
        internal ServiceProvider? serviceProvider;

        /// <summary>
        /// Our encoding client
        /// </summary>
        internal ITokenEvaluatorClient? tokenClient;

        /// <summary>
        /// TikTokenSharp's encoding
        /// </summary>
        internal TikToken tikToken = TikToken.GetEncoding("cl100k_base");

        /// <summary>
        /// SharpToken's encoding
        /// </summary>
        internal GptEncoding encoding = GptEncoding.GetEncoding("cl100k_base");

        /// <summary>
        /// Tiktoken which I think is one of the original implementations.
        /// </summary>
        internal Tiktoken.Encoding ogTiktokenEncoding = Tiktoken.Encoding.Get("cl100k_base");

        [GlobalSetup]
        public void Init()
        {
            // init a service collection, run the extension method to add the library services, and build the service provider
            services.AddTextTokenizationEvaluatorServices();
            services.AddSingleton<ITokenEvaluatorClient, TokenEvaluatorClient>();
            serviceProvider = services.BuildServiceProvider();

            // get the token client.
            tokenClient = serviceProvider.GetService<ITokenEvaluatorClient>();
            tokenClient?.OverridePairedByteEncodingDirectory(Path.Combine(Environment.CurrentDirectory, "TestDataFolder"));

            // Set the encoding type
            tokenClient?.SetDefaultTokenEncoding(EncodingType.Cl100kBase);
        }

        [Benchmark]
        [BenchmarkCategory("EncodeDecode")]
        public void TiktokenSharp_EncodeDecode()
        {
            var encoded = tikToken.Encode(TestConstants.PerformanceFraneknstein);
            var decoded = tikToken.Decode(encoded);
        }

        [Benchmark]
        [BenchmarkCategory("EncodeDecode")]
        public void SharpToken_EncodeDecode()
        {
            var encoded = encoding.Encode(TestConstants.PerformanceFraneknstein);
            var decoded = encoding.Decode(encoded);
        }

        [Benchmark]
        [BenchmarkCategory("EncodeDecode")]
        public void TokenEvaluatorNet_EncodeDecode()
        {
            var encoded = tokenClient.Encode(TestConstants.PerformanceFraneknstein);
            var decoded = tokenClient.Decode(encoded);
        }

        [Benchmark]
        [BenchmarkCategory("EncodeDecode")]
        public void TikToken_EncodeDecode()
        {
            var encoded = ogTiktokenEncoding.Encode(TestConstants.PerformanceFraneknstein);
            var decoded = ogTiktokenEncoding.Decode(encoded);
        }

        [Benchmark]
        [BenchmarkCategory("CountOperations")]
        public void TiktokenSharp_CountSpeed()
        {
            var count = tikToken.Encode(TestConstants.PerformanceFraneknstein)?.Count;
        }

        [Benchmark]
        [BenchmarkCategory("CountOperations")]
        public void SharpToken_CountSpeed()
        {
            var count = encoding.Encode(TestConstants.PerformanceFraneknstein)?.Count;
        }

        [Benchmark]
        [BenchmarkCategory("CountOperations")]
        public void TokenEvaluatorNet_Managed_NonParallel_CountSpeed()
        {
            var count = tokenClient.EncodedTokenCount(TestConstants.PerformanceFraneknstein, false, false);
        }

        [Benchmark]
        [BenchmarkCategory("CountOperations")]
        public void TokenEvaluatorNet_Unsafe_NonParallel_CountSpeed()
        {
            var count = tokenClient.EncodedTokenCount(TestConstants.PerformanceFraneknstein, true, false);
        }

        [Benchmark]
        [BenchmarkCategory("CountOperations")]
        public void TokenEvaluatorNet_Managed_Parallel_CountSpeed()
        {
            var count = tokenClient.EncodedTokenCount(TestConstants.PerformanceFraneknstein, false, false);
        }

        [Benchmark]
        [BenchmarkCategory("CountOperations")]
        public void TokenEvaluatorNet_Unsafe_Parallel_CountSpeed()
        {
            var count = tokenClient.EncodedTokenCount(TestConstants.PerformanceFraneknstein, true, false);
        }

        [Benchmark]
        [BenchmarkCategory("CountOperations")]
        public void TikToken_CountSpeed()
        {
            var count = ogTiktokenEncoding.CountTokens(TestConstants.PerformanceFraneknstein);
        }
    }
}