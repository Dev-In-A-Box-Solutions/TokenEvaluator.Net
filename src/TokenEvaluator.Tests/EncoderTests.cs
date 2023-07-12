using TokenEvaluator.Net;

namespace TokenEvaluator.Tests
{
    [TestClass]
    public class EncoderTests
    {
        internal readonly IServiceCollection services = new ServiceCollection();
        internal ServiceProvider? serviceProvider;
        internal ITokenEvaluatorClient? tokenClient;

        [TestInitialize]
        public void Init()
        {
            // init a service collection, run the extension method to add the library services, and build the service provider
            services.AddTextTokenizationEvaluatorServices();
            services.AddSingleton<ITokenEvaluatorClient, TokenEvaluatorClient>();
            serviceProvider = services.BuildServiceProvider();

            // get the token client.
            tokenClient = serviceProvider.GetService<ITokenEvaluatorClient>();
            tokenClient?.OverridePairedByteEncodingDirectory(Path.Combine(Environment.CurrentDirectory, "TestDataFolder"));
        }

        [TestMethod]
        public void TestCL100kBaseUsingEncodingType()
        {
            if (serviceProvider == null)
            {
                Assert.Fail("Service Provider Null");
            }

            if (tokenClient != null)
            {
                tokenClient.SetDefaultTokenEncoding(EncodingType.Cl100kBase);
                var tokenCount = tokenClient.EncodedTokenCount(Constants.GeneratedText);
                Assert.AreEqual(tokenCount, 45);
            }
            else
            {
                Assert.Fail("Token Client Null");
            }
        }

        [TestMethod]
        public void TestP50kBaseUsingEncodingType()
        {
            if (serviceProvider == null)
            {
                Assert.Fail("Service Provider Null");
            }

            if (tokenClient != null)
            {
                tokenClient.SetDefaultTokenEncoding(EncodingType.P50kBase);
                var tokenCount = tokenClient.EncodedTokenCount(Constants.GeneratedText);
                Assert.AreEqual(tokenCount, 42);
            }
            else
            {
                Assert.Fail("Token Client Null");
            }
        }

        [TestMethod]
        public void TestR50kBaseUsingEncodingType()
        {
            if (serviceProvider == null)
            {
                Assert.Fail("Service Provider Null");
            }

            if (tokenClient != null)
            {
                tokenClient.SetDefaultTokenEncoding(EncodingType.R50kBase);
                var tokenCount = tokenClient.EncodedTokenCount(Constants.GeneratedText);
                Assert.AreEqual(tokenCount, 42);
            }
            else
            {
                Assert.Fail("Token Client Null");
            }
        }

        [TestMethod]
        public void TestTextDavinci003UsingModelType()
        {
            if (serviceProvider == null)
            {
                Assert.Fail("Service Provider Null");
            }

            if (tokenClient != null)
            {
                tokenClient.SetDefaultTokenEncodingForModel(ModelType.TextDavinci003);
                var tokenCount = tokenClient.EncodedTokenCount(Constants.GeneratedText);
                Assert.AreEqual(tokenCount, 42);
            }
            else
            {
                Assert.Fail("Token Client Null");
            }
        }

        [TestMethod]
        public void TestGpt35TurboUsingModelType()
        {
            if (serviceProvider == null)
            {
                Assert.Fail("Service Provider Null");
            }

            if (tokenClient != null)
            {
                tokenClient.SetDefaultTokenEncodingForModel(ModelType.Gpt3_5Turbo);
                var tokenCount = tokenClient.EncodedTokenCount(Constants.GeneratedText);
                Assert.AreEqual(tokenCount, 45);
            }
            else
            {
                Assert.Fail("Token Client Null");
            }
        }

        [TestMethod]
        public void TestUnsafeNativeCount()
        {
            if (serviceProvider == null)
            {
                Assert.Fail("Service Provider Null");
            }

            if (tokenClient != null)
            {
                tokenClient.SetDefaultTokenEncoding(EncodingType.Cl100kBase);
                var tokenCount = tokenClient.EncodedTokenCount(Constants.GeneratedText, true);
                Assert.AreEqual(tokenCount, 45);
            }
            else
            {
                Assert.Fail("Token Client Null");
            }
        }

        [TestMethod]
        public void TestManagedNativeCount()
        {
            if (serviceProvider == null)
            {
                Assert.Fail("Service Provider Null");
            }

            if (tokenClient != null)
            {
                tokenClient.SetDefaultTokenEncoding(EncodingType.Cl100kBase);
                var tokenCount = tokenClient.EncodedTokenCount(Constants.GeneratedText, false);
                Assert.AreEqual(tokenCount, 45);
            }
            else
            {
                Assert.Fail("Token Client Null");
            }
        }

        [TestMethod]
        public void TestCL100kBaseCountUsingParallelProcessing()
        {
            if (serviceProvider == null)
            {
                Assert.Fail("Service Provider Null");
            }

            if (tokenClient != null)
            {
                tokenClient.SetDefaultTokenEncoding(EncodingType.Cl100kBase);
                var tokenCount = tokenClient.EncodedTokenCount(Constants.PerformanceFraneknstein);
                Assert.AreEqual(tokenCount, 5975);
            }
            else
            {
                Assert.Fail("Token Client Null");
            }
        }

        [TestMethod]
        public void TestCL100kBaseCountUsingNonParallelProcessing()
        {
            if (serviceProvider == null)
            {
                Assert.Fail("Service Provider Null");
            }

            if (tokenClient != null)
            {
                tokenClient.SetDefaultTokenEncoding(EncodingType.Cl100kBase);
                var tokenCount = tokenClient.EncodedTokenCount(Constants.PerformanceFraneknstein, false);
                Assert.AreEqual(tokenCount, 5975);
            }
            else
            {
                Assert.Fail("Token Client Null");
            }
        }

        [TestMethod]
        public void TestCL100kBaseEncodeDecodeUsingParallelProcessing()
        {
            if (serviceProvider == null)
            {
                Assert.Fail("Service Provider Null");
            }

            if (tokenClient != null)
            {
                tokenClient.SetDefaultTokenEncoding(EncodingType.Cl100kBase);
                var tokens = tokenClient.Encode(Constants.PerformanceFraneknstein, true);
                var decodedText = tokenClient.Decode(tokens, true);
            }
            else
            {
                Assert.Fail("Token Client Null");
            }
        }

        [TestMethod]
        public void TestCL100kBaseEncodeDecodeUsingNonParallelProcessing()
        {
            if (serviceProvider == null)
            {
                Assert.Fail("Service Provider Null");
            }

            if (tokenClient != null)
            {
                tokenClient.SetDefaultTokenEncoding(EncodingType.Cl100kBase);
                var tokens = tokenClient.Encode(Constants.PerformanceFraneknstein, false);
                var decodedText = tokenClient.Decode(tokens, false);
            }
            else
            {
                Assert.Fail("Token Client Null");
            }
        }

    }
}