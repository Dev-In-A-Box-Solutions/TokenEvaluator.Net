namespace TextTokenizationEvaluator.Tests
{
    [TestClass]
    public class DependencyTests
    {
        [TestMethod]
        public void ServiceCollectionExtension()
        {
            // init a service collection, run the extension method to add the library services, and build the service provider
            IServiceCollection services = new ServiceCollection();
            services.AddTextTokenizationEvaluatorServices();
            var serviceProvider = services.BuildServiceProvider();

            // test we can retrieve the references to the interfaces.
            var encodingService = serviceProvider.GetService<IEncodingService>();
            Assert.IsNotNull(encodingService);
        }
    }
}