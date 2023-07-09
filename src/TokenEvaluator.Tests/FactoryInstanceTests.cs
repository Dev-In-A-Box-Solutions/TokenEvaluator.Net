using TokenEvaluator.Net;

namespace TokenEvaluator.Tests;

[TestClass]
public class FactoryInstanceTests
{
    [TestMethod]
    public async Task FactoryCL100K()
    {
        var client = TokenEvaluatorClientFactory.Create();
        await client.SetDefaultTokenEncodingAsync(EncodingType.Cl100kBase);
        var tokenCount = client.EncodedTokenCount(Constants.GeneratedText);
        Assert.AreEqual(tokenCount, 45);
    }

    [TestMethod]
    public async Task FactoryP50K()
    {
        var client = TokenEvaluatorClientFactory.Create();
        await client.SetDefaultTokenEncodingAsync(EncodingType.P50kBase);
        var tokenCount = client.EncodedTokenCount(Constants.GeneratedText);
        Assert.AreEqual(tokenCount, 42);
    }
}