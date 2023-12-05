using TokenEvaluator.Net;


namespace TokenEvaluator.Tests
{
    [TestClass]
    public class VisionTokenTests
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
        public void HighDetailSquareImageTokenCount()
        {
            if (serviceProvider == null)
            {
                Assert.Fail("Service Provider Null");
            }

            if (tokenClient != null)
            {
                var tokenCount = tokenClient.VisionTokenCount(1040, 1040, DetailLevel.High);
                Assert.AreEqual(tokenCount, 765);
            }
            else
            {
                Assert.Fail("Token Client Null");
            }
        }

        [TestMethod]
        public void HighDetailLandscapeImageTokenCount()
        {
            if (serviceProvider == null)
            {
                Assert.Fail("Service Provider Null");
            }

            if (tokenClient != null)
            {
                var tokenCount = tokenClient.VisionTokenCount(2080, 1040, DetailLevel.High);
                Assert.AreEqual(tokenCount, 1105);
            }
            else
            {
                Assert.Fail("Token Client Null");
            }
        }

        [TestMethod]
        public void HighDetailPortraitImageTokenCount()
        {
            if (serviceProvider == null)
            {
                Assert.Fail("Service Provider Null");
            }

            if (tokenClient != null)
            {
                var tokenCount = tokenClient.VisionTokenCount(2080, 1040, DetailLevel.High);
                Assert.AreEqual(tokenCount, 1105);
            }
            else
            {
                Assert.Fail("Token Client Null");
            }
        }

        [TestMethod]
        public void LowDetailImageTokenCount()
        {
            if (serviceProvider == null)
            {
                Assert.Fail("Service Provider Null");
            }

            if (tokenClient != null)
            {
                var tokenCount = tokenClient.VisionTokenCount(2080, 1040, DetailLevel.Low);
                Assert.AreEqual(tokenCount, 85);
            }
            else
            {
                Assert.Fail("Token Client Null");
            }
        }
    }
}
