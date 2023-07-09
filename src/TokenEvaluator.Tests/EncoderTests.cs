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
        public async Task TestCL100kBaseUsingEncodingType()
        {
            if (serviceProvider == null)
            {
                Assert.Fail("Service Provider Null");
            }

            if (tokenClient != null)
            {
                await tokenClient.SetDefaultTokenEncodingAsync(EncodingType.Cl100kBase);
                var tokenCount = tokenClient.EncodedTokenCount(Constants.GeneratedText);
                Assert.AreEqual(tokenCount, 45);
            }
            else
            {
                Assert.Fail("Token Client Null");
            }
        }

        [TestMethod]
        public async Task TestP50kBaseUsingEncodingType()
        {
            if (serviceProvider == null)
            {
                Assert.Fail("Service Provider Null");
            }

            if (tokenClient != null)
            {
                await tokenClient.SetDefaultTokenEncodingAsync(EncodingType.P50kBase);
                var tokenCount = tokenClient.EncodedTokenCount(Constants.GeneratedText);
                Assert.AreEqual(tokenCount, 42);
            }
            else
            {
                Assert.Fail("Token Client Null");
            }
        }

        [TestMethod]
        public async Task TestTextDavinci003UsingModelType()
        {
            if (serviceProvider == null)
            {
                Assert.Fail("Service Provider Null");
            }

            if (tokenClient != null)
            {
                await tokenClient.SetDefaultTokenEncodingForModelAsync(ModelType.TextDavinci003);
                var tokenCount = tokenClient.EncodedTokenCount(Constants.GeneratedText);
                Assert.AreEqual(tokenCount, 42);
            }
            else
            {
                Assert.Fail("Token Client Null");
            }
        }

        [TestMethod]
        public async Task TestGpt35TurboUsingModelType()
        {
            if (serviceProvider == null)
            {
                Assert.Fail("Service Provider Null");
            }

            if (tokenClient != null)
            {
                await tokenClient.SetDefaultTokenEncodingForModelAsync(ModelType.Gpt3_5Turbo);
                var tokenCount = tokenClient.EncodedTokenCount(Constants.GeneratedText);
                Assert.AreEqual(tokenCount, 45);
            }
            else
            {
                Assert.Fail("Token Client Null");
            }
        }
    }
}