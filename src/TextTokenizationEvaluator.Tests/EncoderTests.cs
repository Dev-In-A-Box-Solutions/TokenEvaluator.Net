using TextTokenizationEvaluator.Core;

namespace TextTokenizationEvaluator.Tests
{
    [TestClass]
    public class EncoderTests
    {
        internal readonly IServiceCollection services = new ServiceCollection();
        internal ServiceProvider? serviceProvider;

        [TestInitialize]
        public void Init()
        {
            // init a service collection, run the extension method to add the library services, and build the service provider
            services.AddTextTokenizationEvaluatorServices();
            services.AddSingleton<ITokenEvaluatorClient, TokenEvaluatorClient>();
            serviceProvider = services.BuildServiceProvider();
        }

        [TestMethod]
        public void TestC100kBase()
        {
            if (serviceProvider == null)
            {
                Assert.Fail("Service Provider Null");
            }

            // test we can retrieve the references to the interfaces.
            var tokenClient = serviceProvider.GetService<ITokenEvaluatorClient>();
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
        public void TestP50kBase()
        {
            if (serviceProvider == null)
            {
                Assert.Fail("Service Provider Null");
            }

            // test we can retrieve the references to the interfaces.
            var tokenClient = serviceProvider.GetService<ITokenEvaluatorClient>();
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
    }
}