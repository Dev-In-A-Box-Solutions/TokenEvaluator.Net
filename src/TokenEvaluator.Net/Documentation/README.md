# TokenEvaluator.Net

![repoTemplate](https://github.com/Dev-In-A-Box-Solutions/TokenEvaluator.Net/assets/17493722/c3c9425e-2457-417f-9075-daec954d5bba)


### Build Status
[![main](https://github.com/JoeTomkinson/TokenEvaluator.Net/actions/workflows/main-build.yml/badge.svg)](https://github.com/JoeTomkinson/TokenEvaluator.Net/actions/workflows/main-build.yml)
[![develop](https://github.com/JoeTomkinson/TokenEvaluator.Net/actions/workflows/develop-build.yml/badge.svg)](https://github.com/JoeTomkinson/TokenEvaluator.Net/actions/workflows/develop-build.yml)

These are currently failing due to an issue with GitHub action directories, working to resolve that at the moment.

## Description
TokenEvaluator.Net is a simple and useful library designed to measure and calculate the token count of given text inputs, as per the specifics of the language model specified by the user. This tool is crucial for efficient resource management when dealing with AI language models, such as OpenAI's GPT-3.5-turbo and others.

By providing a comprehensive and detailed evaluation of the token count, this library assists developers in understanding the cost, performance, and optimization aspects of their AI language model interactions.

Whether you're running an AI chatbot, a content generator, or any application that leverages AI language models, understanding your token usage is fundamental. TokenEvaluator.Net fills this gap, offering a clear, accurate, and easy-to-use solution.

## Features:

1. **Precise token count calculations** aligned with the specified language model
2. Support for a diverse **array of popular AI language models**
3. **Efficient and lightweight architecture** suitable for both integrated and standalone usage
4. **Open-source**, fostering community contributions and ongoing enhancement

Unlock the power of accurate token count understanding with TokenEvaluator.Net - the essential tool for AI developers.

## Supported Tokenizers
These are the currently supported tokenizers:

- CL100K
- P50K
- r50k

## NuGet Packages

![logo64](https://github.com/Dev-In-A-Box-Solutions/TokenEvaluator.Net/assets/17493722/0fc0e333-1abe-4f54-8ad7-53ee7f7b8a9d)

https://www.nuget.org/packages/TokenEvaluator.Net

# Benchmarking

Last Ran 11/07/2023 - TokenEvaluator.NET - Version 1.0.5
    
### Environment

BenchmarkDotNet=v0.13.5, OS=Windows 11 (10.0.22621.1848/22H2/2022Update/SunValley2)
11th Gen Intel Core i7-11700K 3.60GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK= 7.0.304
[Host]     : .NET 7.0.7 (7.0.723.27404), X64 RyuJIT AVX2[AttachedDebugger]
DefaultJob : .NET 7.0.7 (7.0.723.27404), X64 RyuJIT AVX2

### Variables

- Tokeniser: cl100k_base
- Text Token Length: 5412 Tokens (cl100k_base)

### Memory Tests
    
|                               Method |       Mean |    Error |   StdDev |     Gen0 |     Gen1 |  Allocated |
|------------------------------------- |-----------:|---------:|---------:|---------:|---------:|-----------:|
|           TiktokenSharp_EncodeDecode | 1,281.4 us | 24.88 us | 27.65 us | 257.8125 | 148.4375 | 2117.04 KB |
|              SharpToken_EncodeDecode | 1,796.2 us | 35.71 us | 39.69 us | 285.1563 | 146.4844 | 2344.87 KB |
|       TokenEvaluatorNet_EncodeDecode | 1,298.5 us | 23.28 us | 20.64 us | 240.2344 | 142.5781 | 1978.21 KB |
|                TikToken_EncodeDecode |   671.7 us | 12.64 us | 13.53 us |  67.3828 |   8.7891 |  555.31 KB |
|             TiktokenSharp_CountSpeed | 1,041.9 us | 20.17 us | 24.01 us | 220.7031 | 115.2344 |  1803.3 KB |
|                SharpToken_CountSpeed | 1,747.7 us | 24.31 us | 18.98 us | 269.5313 | 140.6250 | 2212.15 KB |
| TokenEvaluatorNet_Managed_CountSpeed | 1,066.2 us |  6.66 us |  5.56 us | 210.9375 | 103.5156 | 1727.02 KB |
|  TokenEvaluatorNet_Unsafe_CountSpeed |   417.4 us |  2.33 us |  1.94 us |  42.4805 |        - |  347.79 KB |
|                  TikToken_CountSpeed |   429.3 us |  5.58 us |  5.22 us |  42.4805 |        - |  347.79 KB |
    
#### Legends
- Mean      : Arithmetic mean of all measurements
- Error     : Half of 99.9% confidence interval
- StdDev    : Standard deviation of all measurements
- Gen0      : GC Generation 0 collects per 1000 operations
- Gen1      : GC Generation 1 collects per 1000 operations
- Allocated : Allocated memory per single operation(managed only, inclusive, 1KB = 1024B)
- 1 us      : 1 Microsecond(0.000001 sec)


# Getting Started

TokenEvaluator.Net can be used via dependency injection, or an instance can be created using a tightly-coupled factory class.

### Dependency Injection
If you want to be able to inject an instance of this client into multiple methods, then you can make use of the libraries dependency injection extension to add all of the required interfaces and implementations to your service collection.

```C#
using TokenEvaluator.Net.Dependency;

internal readonly IServiceCollection services = new ServiceCollection();
internal ServiceProvider? serviceProvider;
internal ITokenEvaluatorClient? tokenClient;

// init a service collection, run the extension method to add the library services, and build the service provider
services.AddTextTokenizationEvaluatorServices();
services.AddSingleton<ITokenEvaluatorClient, TokenEvaluatorClient>();
serviceProvider = services.BuildServiceProvider();

// get the token client.
tokenClient = serviceProvider.GetService<ITokenEvaluatorClient>();
tokenClient?.OverridePairedByteEncodingDirectory(Path.Combine(Environment.CurrentDirectory, "TestDataFolder"));
```

Then simply inject the service into your class constructors like so:

```C#
internal const string GeneratedText = "The quick, brown fox—enamoured by the moonlit night—jumped over 10 lazily sleeping dogs near 123 Elm St. at approximately 7:30 PM. Isn't text tokenization interesting?";

public ClassConstructor(ITokenEvaluatorClient tokenClient)
{
    // Set token encoding type
    await tokenClient.SetDefaultTokenEncodingAsync(EncodingType.Cl100kBase);
    var tokenCount = tokenClient.EncodedTokenCount(GeneratedText);
}
```

### Factory Implementation

Using this as a concrete, tightly-coupled implementation is fairly straightforward. Simply use the below code and all internal interface and service references will be initialised and tightly-coupled. This is difficult to write tests for within your application, but ultimately is the easiest way to implement the client.

```C#

using TokenEvaluator.Net;

var client = TokenEvaluatorClientFactory.Create();
await client.SetDefaultTokenEncodingAsync(EncodingType.Cl100kBase);
var tokenCount = client.EncodedTokenCount(GeneratedText);
```