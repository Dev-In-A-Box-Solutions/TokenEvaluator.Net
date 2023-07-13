# TokenEvaluator.Net

![repoTemplate](https://github.com/Dev-In-A-Box-Solutions/TokenEvaluator.Net/assets/17493722/c3c9425e-2457-417f-9075-daec954d5bba)


### Build Status
[![main](https://github.com/JoeTomkinson/TokenEvaluator.Net/actions/workflows/main-build.yml/badge.svg)](https://github.com/JoeTomkinson/TokenEvaluator.Net/actions/workflows/main-build.yml)
[![develop](https://github.com/JoeTomkinson/TokenEvaluator.Net/actions/workflows/develop-build.yml/badge.svg)](https://github.com/JoeTomkinson/TokenEvaluator.Net/actions/workflows/develop-build.yml)

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
- R50K

## NuGet Packages

![logo64](https://github.com/Dev-In-A-Box-Solutions/TokenEvaluator.Net/assets/17493722/0fc0e333-1abe-4f54-8ad7-53ee7f7b8a9d)

https://www.nuget.org/packages/TokenEvaluator.Net

# Getting Started

TokenEvaluator.Net can be used via dependency injection, or an instance can be created using a tightly-coupled factory class.

### Dependency Injection
If you want to be able to inject an instance of this client into multiple methods, then you can make use of the libraries dependency injection extension to add all of the required interfaces and implementations to your service collection.

```C#
using TokenEvaluator.Net.Dependency;

// Init a service collection, use the extension method to add the library services.
IServiceCollection services = new ServiceCollection();
services.AddTokenEvaluator.NetServices();
services.AddSingleton<ITokenEvaluatorClient, TokenEvaluatorClient>();
var serviceProvider = services.BuildServiceProvider();
```

Then simply inject the service into your class constructors like so:

```C#
internal const string GeneratedText = "The quick, brown fox—enamored by the moonlit night—jumped over 10 lazily sleeping dogs near 123 Elm St. at approximately 7:30 PM. Isn't text tokenization interesting?";

public ClassConstructor(ITokenEvaluatorClient tokenClient)
{
    // Set token encoding type
    tokenClient.SetDefaultTokenEncoding(EncodingType.Cl100kBase);
    var tokenCount = tokenClient.EncodedTokenCount(GeneratedText);

    // or choose a supported model
    tokenClient.SetDefaultTokenEncodingForModel(ModelType.TextDavinci003);
    var tokenCount = tokenClient.EncodedTokenCount(GeneratedText);
}
```

### Factory Implementation

Using this as a concrete, tightly-coupled implementation is fairly straightforward. Simply use the below code and all internal interface and service references will be initialised and tightly-coupled. This is difficult to write tests for within your application, but ultimately is the easiest way to implement the client.

```C#

using TokenEvaluator.Net;

var client = TokenEvaluatorClientFactory.Create();
client.SetDefaultTokenEncoding(EncodingType.Cl100kBase);
var tokenCount = client.EncodedTokenCount(Constants.GeneratedText);
```

### Unsafe Encoding

EncodedTokenCount allows the use of unsafe encoding, these methods use the unsafe keyword and are not recommended for use in production environments. They are however useful for benchmarking and testing purposes; refer to the Microsoft documentation on unsafe code for more information.
[Microsoft Docs: Unsafe code, pointer types, and function pointers](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/unsafe-code]

the 'unsafe' parameter defaults to false, but can be set to true if required.

```C#
internal const string GeneratedText = "The quick, brown fox—enamored by the moonlit night—jumped over 10 lazily sleeping dogs near 123 Elm St. at approximately 7:30 PM. Isn't text tokenization interesting?";

public ClassConstructor(ITokenEvaluatorClient tokenClient)
{
    // Set token encoding type
    tokenClient.SetDefaultTokenEncoding(EncodingType.Cl100kBase);
    var tokenCount = tokenClient.EncodedTokenCount(GeneratedText, unsafe: true, useParallelProcessing: false);

    // or choose a supported model
    tokenClient.SetDefaultTokenEncodingForModel(ModelType.TextDavinci003);
    var tokenCount = tokenClient.EncodedTokenCount(GeneratedText, unsafe: true, useParallelProcessing: false);
}
```

### Parallel Encoding

EncodedTokenCount, Encode, and Decode Methods allow developers to make use of parallel processing (This utilises the parallel threading library), this is useful for large text inputs and can significantly reduce the time taken to encode the text. This is not recommended for use in production environments, but is useful for benchmarking and testing purposes.

The 'useParallelProcessing' parameter defaults to true, but can be set to false if required.

```C#
internal const string GeneratedText = "The quick, brown fox—enamored by the moonlit night—jumped over 10 lazily sleeping dogs near 123 Elm St. at approximately 7:30 PM. Isn't text tokenization interesting?";

public ClassConstructor(ITokenEvaluatorClient tokenClient)
{
    // Set token encoding type
    tokenClient.SetDefaultTokenEncoding(EncodingType.Cl100kBase);
    var tokenCount = tokenClient.EncodedTokenCount(GeneratedText, unsafe: false,useParallelProcessing: true);
    var tokens = tokenClient.Encode(GeneratedText, useParallelProcessing: true);
    var decodedText = tokenClient.Decode(tokens, useParallelProcessing: true);
}
```

# Benchmarking

For the purposes of openness and transparency included below are a number of benchmark tests. The library used to run these is included within the Benchmark folder.

- Speed tests focused on encoding and then decoding.
- Count tests focused on encoding only whilst returning the token count.

These need some review and assessment to determine realistically which is the most efficient, but the results are included below for reference.


```
BenchmarkDotNet v0.13.6, Windows 11 (10.0.22621.1848/22H2/2022Update/SunValley2)
11th Gen Intel Core i7-11700K 3.60GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 7.0.304
  [Host]     : .NET 7.0.7 (7.0.723.27404), X64 RyuJIT AVX2 [AttachedDebugger]
  DefaultJob : .NET 7.0.7 (7.0.723.27404), X64 RyuJIT AVX2
```

### Token Count

|                                           Method |      Categories |       Mean |   StdErr |        Min |         Q1 |     Median |         Q3 |        Max |    Op/s |     Gen0 |     Gen1 |  Allocated |
|------------------------------------------------- |---------------- |-----------:|---------:|-----------:|-----------:|-----------:|-----------:|-----------:|--------:|---------:|---------:|-----------:|
|                         TiktokenSharp_CountSpeed | CountOperations | 1,574.6 μs |  8.00 μs | 1,526.2 μs | 1,541.8 μs | 1,563.4 μs | 1,612.6 μs | 1,639.0 μs |   635.1 | 248.0469 | 177.7344 | 2031.03 KB |
|                            SharpToken_CountSpeed | CountOperations | 2,377.3 μs | 12.08 μs | 2,290.6 μs | 2,331.4 μs | 2,360.9 μs | 2,422.3 μs | 2,498.6 μs |   420.6 | 324.2188 | 226.5625 | 2658.29 KB |
| TokenEvaluatorNet_Managed_NonParallel_CountSpeed | CountOperations | 1,621.9 μs |  8.18 μs | 1,543.0 μs | 1,605.4 μs | 1,616.0 μs | 1,644.8 μs | 1,693.9 μs |   616.6 | 238.2813 | 171.8750 | 1954.78 KB |
|  TokenEvaluatorNet_Unsafe_NonParallel_CountSpeed | CountOperations |   597.7 μs |  2.79 μs |   576.0 μs |   592.1 μs |   595.3 μs |   603.6 μs |   617.0 μs | 1,673.0 |  47.8516 |        - |  391.77 KB |
|    TokenEvaluatorNet_Managed_Parallel_CountSpeed | CountOperations | 1,640.2 μs |  6.53 μs | 1,592.7 μs | 1,617.5 μs | 1,638.2 μs | 1,655.3 μs | 1,686.8 μs |   609.7 | 238.2813 | 171.8750 | 1954.78 KB |
|     TokenEvaluatorNet_Unsafe_Parallel_CountSpeed | CountOperations |   593.6 μs |  1.69 μs |   578.7 μs |   590.8 μs |   592.9 μs |   597.7 μs |   602.8 μs | 1,684.6 |  47.8516 |        - |  391.77 KB |
|                              TikToken_CountSpeed | CountOperations |   604.6 μs |  2.04 μs |   592.2 μs |   597.3 μs |   605.3 μs |   609.9 μs |   620.2 μs | 1,653.9 |  47.8516 |        - |  391.74 KB |

### Encode/Decode

|                                           Method |      Categories |       Mean |   StdErr |        Min |         Q1 |     Median |         Q3 |        Max |    Op/s |     Gen0 |     Gen1 |  Allocated |
|------------------------------------------------- |---------------- |-----------:|---------:|-----------:|-----------:|-----------:|-----------:|-----------:|--------:|---------:|---------:|-----------:|
|                       TiktokenSharp_EncodeDecode |    EncodeDecode | 1,841.2 μs |  8.96 μs | 1,789.1 μs | 1,809.6 μs | 1,836.9 μs | 1,867.7 μs | 1,921.1 μs |   543.1 | 289.0625 | 164.0625 | 2383.89 KB |
|                          SharpToken_EncodeDecode |    EncodeDecode | 2,717.7 μs | 15.66 μs | 2,531.2 μs | 2,634.8 μs | 2,705.1 μs | 2,802.7 μs | 3,076.4 μs |   368.0 | 339.8438 | 226.5625 | 2802.97 KB |
|                   TokenEvaluatorNet_EncodeDecode |    EncodeDecode | 2,559.7 μs | 14.49 μs | 2,356.4 μs | 2,501.5 μs | 2,547.7 μs | 2,625.6 μs | 2,811.4 μs |   390.7 | 375.0000 | 371.0938 | 3005.72 KB |
|                     TikToken_Unsafe_EncodeDecode |    EncodeDecode |   934.8 μs |  4.87 μs |   895.7 μs |   919.7 μs |   929.4 μs |   946.1 μs |   980.7 μs | 1,069.8 |  75.1953 |   7.8125 |  618.92 KB |
