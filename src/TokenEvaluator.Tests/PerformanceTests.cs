using System.Diagnostics;
using TokenEvaluator.Net;

namespace TokenEvaluator.Tests;

[TestClass]
public class PerformanceTests
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
    public void CL100KPerformanceSpeed()
    {
        if (serviceProvider == null)
        {
            Assert.Fail("Service Provider Null");
        }

        if (tokenClient == null)
        {
            Assert.Fail("Token Client Null");
        }

        // Set the encoding type
        tokenClient.SetDefaultTokenEncoding(EncodingType.Cl100kBase);

        // Prepare the Stopwatch
        var stopwatch = new Stopwatch();

        // Start timing
        stopwatch.Start();

        // Perform the operation you want to time
        var tokenCount = tokenClient.EncodedTokenCount(Constants.PerformanceTestString);

        // Stop timing
        stopwatch.Stop();

        // log token count
        Trace.WriteLine($"Token Count: {tokenCount}");

        // Print out or do something with the elapsed time
        Trace.WriteLine($"Elapsed time: {stopwatch.ElapsedMilliseconds} ms");
    }

    [TestMethod]
    public void CL100KPerformanceMemoryUsage()
    {
        if (serviceProvider == null)
        {
            Assert.Fail("Service Provider Null");
        }

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

        tokenClient.SetDefaultTokenEncoding(EncodingType.Cl100kBase); 
        var tokenCount = tokenClient.EncodedTokenCount(Constants.PerformanceTestString);

        // Force another garbage collection and get the end memory usage
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();

        long endMemory = GC.GetTotalMemory(true);

        // Calculate the difference
        long memoryUsed = endMemory - startMemory;

        // log token count
        Trace.WriteLine($"Token Count: {tokenCount}");

        Trace.WriteLine($"Memory used: {memoryUsed} bytes");
    }

    [TestMethod]
    public void CL100KPerformanceCPU()
    {
        if (serviceProvider == null)
        {
            Assert.Fail("Service Provider Null");
        }

        if (tokenClient == null)
        {
            Assert.Fail("Token Client Null");
        }

        var currentProcess = Process.GetCurrentProcess();

        // Get the current CPU usage for the process
        var startCpuUsage = currentProcess.TotalProcessorTime;

        tokenClient.SetDefaultTokenEncoding(EncodingType.Cl100kBase);

        // Perform the operation you want to measure
        var tokenCount = tokenClient.EncodedTokenCount(Constants.PerformanceTestString);

        // Calculate CPU usage
        var endCpuUsage = currentProcess.TotalProcessorTime;
        var cpuUsed = endCpuUsage - startCpuUsage;

        // log token count
        Trace.WriteLine($"Token Count: {tokenCount}");

        Trace.WriteLine($"CPU used: {cpuUsed.TotalMilliseconds} milliseconds");
    }

    [TestMethod]
    public void P50KPerformanceSpeed()
    {
        if (serviceProvider == null)
        {
            Assert.Fail("Service Provider Null");
        }

        if (tokenClient == null)
        {
            Assert.Fail("Token Client Null");
        }

        // Set the encoding type
        tokenClient.SetDefaultTokenEncoding(EncodingType.P50kBase);

        // Prepare the Stopwatch
        var stopwatch = new Stopwatch();

        // Start timing
        stopwatch.Start();

        // Perform the operation you want to time
        var tokenCount = tokenClient.EncodedTokenCount(Constants.PerformanceTestString);

        // Stop timing
        stopwatch.Stop();

        // log token count
        Trace.WriteLine($"Token Count: {tokenCount}");

        // Print out or do something with the elapsed time
        Trace.WriteLine($"Elapsed time: {stopwatch.ElapsedMilliseconds} ms");
    }

    [TestMethod]
    public void P50KPerformanceMemoryUsage()
    {
        if (serviceProvider == null)
        {
            Assert.Fail("Service Provider Null");
        }

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

        tokenClient.SetDefaultTokenEncoding(EncodingType.P50kBase);

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
        Trace.WriteLine($"Token Count: {tokenCount}");

        Trace.WriteLine($"Memory used: {memoryUsed} bytes");
    }

    [TestMethod]
    public void P50KPerformanceCPU()
    {
        if (serviceProvider == null)
        {
            Assert.Fail("Service Provider Null");
        }

        if (tokenClient == null)
        {
            Assert.Fail("Token Client Null");
        }

        var currentProcess = Process.GetCurrentProcess();

        // Get the current CPU usage for the process
        var startCpuUsage = currentProcess.TotalProcessorTime;

        tokenClient.SetDefaultTokenEncoding(EncodingType.P50kBase);

        // Perform the operation you want to measure
        var tokenCount = tokenClient.EncodedTokenCount(Constants.PerformanceTestString);

        // Calculate CPU usage
        var endCpuUsage = currentProcess.TotalProcessorTime;
        var cpuUsed = endCpuUsage - startCpuUsage;

        // log token count
        Trace.WriteLine($"Token Count: {tokenCount}");

        Trace.WriteLine($"CPU used: {cpuUsed.TotalMilliseconds} milliseconds");
    }
}