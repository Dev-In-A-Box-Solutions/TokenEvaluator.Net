using System.IO;
using System.Reflection;

namespace TokenEvaluator.Net.Services.Contracts
{
    public interface IEmbeddedResourceQuery
    {
        Stream? Read<T>(string resource);
        Stream? Read(Assembly assembly, string resource);
        Stream? Read(string assemblyName, string resource);
    }
}