﻿using TokenEvaluator.Net;

namespace TokenEvaluator.Tests;

[TestClass]
public class FactoryInstanceTests
{
    [TestMethod]
    public void FactoryCL100K()
    {
        var client = TokenEvaluatorClientFactory.Create();
        client.SetDefaultTokenEncoding(EncodingType.Cl100kBase);
        var tokenCount = client.EncodedTokenCount(Constants.GeneratedText);
        Assert.AreEqual(tokenCount, 45);
    }

    [TestMethod]
    public void FactoryP50K()
    {
        var client = TokenEvaluatorClientFactory.Create();
        client.SetDefaultTokenEncoding(EncodingType.R50kBase);
        var tokenCount = client.EncodedTokenCount(Constants.GeneratedText);
        Assert.AreEqual(tokenCount, 42);
    }
}