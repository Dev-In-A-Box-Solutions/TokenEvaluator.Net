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
            _ = tikToken.Decode(encoded);
        }

        [Benchmark]
        [BenchmarkCategory("EncodeDecode")]
        public void SharpToken_EncodeDecode()
        {
            var encoded = encoding.Encode(TestConstants.PerformanceFraneknstein);
            _ = encoding.Decode(encoded);
        }

        [Benchmark]
        [BenchmarkCategory("EncodeDecode")]
        public void TokenEvaluatorNet_EncodeDecode()
        {
            var encoded = tokenClient.Encode(TestConstants.PerformanceFraneknstein);
            _ = tokenClient.Decode(encoded);
        }

        [Benchmark]
        [BenchmarkCategory("EncodeDecode")]
        public void TikToken_EncodeDecode()
        {
            var encoded = ogTiktokenEncoding.Encode(TestConstants.PerformanceFraneknstein);
            _ = ogTiktokenEncoding.Decode(encoded);
        }

        [Benchmark]
        [BenchmarkCategory("CountOperations")]
        public void TiktokenSharp_CountSpeed()
        {
            _ = tikToken.Encode(TestConstants.PerformanceFraneknstein)?.Count;
        }

        [Benchmark]
        [BenchmarkCategory("CountOperations")]
        public void SharpToken_CountSpeed()
        {
            _ = encoding.Encode(TestConstants.PerformanceFraneknstein)?.Count;
        }

        [Benchmark]
        [BenchmarkCategory("CountOperations")]
        public void TokenEvaluatorNet_Safe_NonParallel_CountSpeed()
        {
            _ = tokenClient.EncodedTokenCount(TestConstants.PerformanceFraneknstein, false, false);
        }

        [Benchmark]
        [BenchmarkCategory("CountOperations")]
        public void TokenEvaluatorNet_Unsafe_NonParallel_CountSpeed()
        {
            _ = tokenClient.EncodedTokenCount(TestConstants.PerformanceFraneknstein, true, false);
        }

        [Benchmark]
        [BenchmarkCategory("CountOperations")]
        public void TokenEvaluatorNet_Safe_Parallel_CountSpeed()
        {
            _ = tokenClient.EncodedTokenCount(TestConstants.PerformanceFraneknstein, false, false);
        }

        [Benchmark]
        [BenchmarkCategory("CountOperations")]
        public void TokenEvaluatorNet_Unsafe_Parallel_CountSpeed()
        {
            _ = tokenClient.EncodedTokenCount(TestConstants.PerformanceFraneknstein, true, false);
        }

        [Benchmark]
        [BenchmarkCategory("CountOperations")]
        public void TikToken_CountSpeed()
        {
            _ = ogTiktokenEncoding.CountTokens(TestConstants.PerformanceFraneknstein);
        }

        #region TokenEvaluator Large String

        [Benchmark]
        [BenchmarkCategory("TokenEvaluator")]
        public void EncodeDecode()
        {
            var encoded = tokenClient.Encode(TestConstants.PerformanceTestString);
            _ = tokenClient.Decode(encoded);
        }

        [Benchmark]
        [BenchmarkCategory("TokenEvaluator")]
        public void Safe_NonParallel_CountSpeed()
        {
            _ = tokenClient.EncodedTokenCount(TestConstants.PerformanceTestString, false, false);
        }

        [Benchmark]
        [BenchmarkCategory("TokenEvaluator")]
        public void Unsafe_NonParallel_CountSpeed()
        {
            _ = tokenClient.EncodedTokenCount(TestConstants.PerformanceTestString, true, false);
        }

        [Benchmark]
        [BenchmarkCategory("TokenEvaluator")]
        public void Safe_Parallel_CountSpeed()
        {
            _ = tokenClient.EncodedTokenCount(TestConstants.PerformanceTestString, false, false);
        }

        [Benchmark]
        [BenchmarkCategory("TokenEvaluator")]
        public void Unsafe_Parallel_CountSpeed()
        {
            _ = tokenClient.EncodedTokenCount(TestConstants.PerformanceTestString, true, false);
        }
        #endregion
    }
}