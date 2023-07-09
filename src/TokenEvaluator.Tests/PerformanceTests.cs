using System.Diagnostics;
using TokenEvaluator.Net;

namespace TokenEvaluator.Tests;

[TestClass]
public class PerformanceTests
{
    internal readonly IServiceCollection services = new ServiceCollection();
    internal ServiceProvider? serviceProvider;

    [TestInitialize]
    public void Init()
    {
        services.AddTextTokenizationEvaluatorServices();
        services.AddSingleton<ITokenEvaluatorClient, TokenEvaluatorClient>();
        serviceProvider = services.BuildServiceProvider();
    }

    [TestMethod]
    public async Task CL100KPerformanceSpeed()
    {
        if (serviceProvider == null)
        {
            Assert.Fail("Service Provider Null");
        }

        var tokenClient = serviceProvider.GetService<ITokenEvaluatorClient>();
        if (tokenClient == null)
        {
            Assert.Fail("Token Client Null");
        }

        // Set the encoding type
        await tokenClient.SetDefaultTokenEncodingAsync(EncodingType.Cl100kBase);

        // Prepare the Stopwatch
        var stopwatch = new Stopwatch();

        // Start timing
        stopwatch.Start();

        // Perform the operation you want to time
        var tokenCount = tokenClient.EncodedTokenCount(Constants.PerformanceTestString);

        // Stop timing
        stopwatch.Stop();

        // log token count
        Debug.WriteLine($"Token Count: {tokenCount}");

        // Print out or do something with the elapsed time
        Debug.WriteLine($"Elapsed time: {stopwatch.ElapsedMilliseconds} ms");
    }

    [TestMethod]
    public async Task CL100KPerformanceMemoryUsage()
    {
        if (serviceProvider == null)
        {
            Assert.Fail("Service Provider Null");
        }

        var tokenClient = serviceProvider.GetService<ITokenEvaluatorClient>();
        if (tokenClient == null)
        {
            Assert.Fail("Token Client Null");
        }

        // Force a garbage collection to minimize noise from other parts of the application
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();

        // Get the current memory usage
        long startMemory = GC.GetTotalMemory(true);

        await tokenClient.SetDefaultTokenEncodingAsync(EncodingType.Cl100kBase); 
        var tokenCount = tokenClient.EncodedTokenCount(Constants.PerformanceTestString);

        // Force another garbage collection and get the end memory usage
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();

        long endMemory = GC.GetTotalMemory(true);

        // Calculate the difference
        long memoryUsed = endMemory - startMemory;

        // log token count
        Debug.WriteLine($"Token Count: {tokenCount}");

        Debug.WriteLine($"Memory used: {memoryUsed} bytes");
    }

    [TestMethod]
    public async Task CL100KPerformanceCPU()
    {
        if (serviceProvider == null)
        {
            Assert.Fail("Service Provider Null");
        }

        var tokenClient = serviceProvider.GetService<ITokenEvaluatorClient>();
        if (tokenClient == null)
        {
            Assert.Fail("Token Client Null");
        }

        var currentProcess = Process.GetCurrentProcess();

        // Get the current CPU usage for the process
        var startCpuUsage = currentProcess.TotalProcessorTime;

        await tokenClient.SetDefaultTokenEncodingAsync(EncodingType.Cl100kBase);

        // Perform the operation you want to measure
        var tokenCount = tokenClient.EncodedTokenCount(Constants.PerformanceTestString);

        // Calculate CPU usage
        var endCpuUsage = currentProcess.TotalProcessorTime;
        var cpuUsed = endCpuUsage - startCpuUsage;

        // log token count
        Debug.WriteLine($"Token Count: {tokenCount}");

        Debug.WriteLine($"CPU used: {cpuUsed.TotalMilliseconds} milliseconds");
    }

    [TestMethod]
    public async Task P50KPerformanceSpeed()
    {
        if (serviceProvider == null)
        {
            Assert.Fail("Service Provider Null");
        }

        var tokenClient = serviceProvider.GetService<ITokenEvaluatorClient>();
        if (tokenClient == null)
        {
            Assert.Fail("Token Client Null");
        }

        // Set the encoding type
        await tokenClient.SetDefaultTokenEncodingAsync(EncodingType.P50kBase);

        // Prepare the Stopwatch
        var stopwatch = new Stopwatch();

        // Start timing
        stopwatch.Start();

        // Perform the operation you want to time
        var tokenCount = tokenClient.EncodedTokenCount(Constants.PerformanceTestString);

        // Stop timing
        stopwatch.Stop();

        // log token count
        Debug.WriteLine($"Token Count: {tokenCount}");

        // Print out or do something with the elapsed time
        Debug.WriteLine($"Elapsed time: {stopwatch.ElapsedMilliseconds} ms");
    }

    [TestMethod]
    public async Task P50KPerformanceMemoryUsage()
    {
        if (serviceProvider == null)
        {
            Assert.Fail("Service Provider Null");
        }

        var tokenClient = serviceProvider.GetService<ITokenEvaluatorClient>();
        if (tokenClient == null)
        {
            Assert.Fail("Token Client Null");
        }

        // Force a garbage collection to minimize noise from other parts of the application
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();

        // Get the current memory usage
        long startMemory = GC.GetTotalMemory(true);

        await tokenClient.SetDefaultTokenEncodingAsync(EncodingType.P50kBase);

        // Perform the operation you want to measure
        var tokenCount = tokenClient.EncodedTokenCount(Constants.PerformanceTestString);

        // Force another garbage collection and get the end memory usage
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();

        long endMemory = GC.GetTotalMemory(true);

        // Calculate the difference
        long memoryUsed = endMemory - startMemory;

        // log token count
        Debug.WriteLine($"Token Count: {tokenCount}");

        Debug.WriteLine($"Memory used: {memoryUsed} bytes");
    }

    [TestMethod]
    public async Task P50KPerformanceCPU()
    {
        if (serviceProvider == null)
        {
            Assert.Fail("Service Provider Null");
        }

        var tokenClient = serviceProvider.GetService<ITokenEvaluatorClient>();
        if (tokenClient == null)
        {
            Assert.Fail("Token Client Null");
        }

        var currentProcess = Process.GetCurrentProcess();

        // Get the current CPU usage for the process
        var startCpuUsage = currentProcess.TotalProcessorTime;

        await tokenClient.SetDefaultTokenEncodingAsync(EncodingType.P50kBase);

        // Perform the operation you want to measure
        var tokenCount = tokenClient.EncodedTokenCount(Constants.PerformanceTestString);

        // Calculate CPU usage
        var endCpuUsage = currentProcess.TotalProcessorTime;
        var cpuUsed = endCpuUsage - startCpuUsage;

        // log token count
        Debug.WriteLine($"Token Count: {tokenCount}");

        Debug.WriteLine($"CPU used: {cpuUsed.TotalMilliseconds} milliseconds");
    }
}