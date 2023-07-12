using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using TokenEvaluator.Net.Services.Contracts;
using TokenEvaluator.Net.Utils;

namespace TokenEvaluator.Net.Services
{
    internal class EmbeddedResourceQuery : IEmbeddedResourceQuery
    {
        private readonly Dictionary<Assembly, string> _assemblyNames;

        public EmbeddedResourceQuery() : this(Array.Empty<Assembly>())
        {

        }

        public EmbeddedResourceQuery(IEnumerable<Assembly> assembliesToPreload)
        {
            _assemblyNames = new Dictionary<Assembly, string>(new AssemblyComparer());
            foreach (var assembly in assembliesToPreload)
            {
                _assemblyNames.Add(assembly, assembly.GetName().Name!);
            }
        }

        public Stream? Read<T>(string resource)
        {
            var assembly = typeof(T).Assembly;
            return ReadInternal(assembly, resource);
        }

        public Stream? Read(Assembly assembly, string resource)
        {
            return ReadInternal(assembly, resource);
        }

        public Stream? Read(string assemblyName, string resource)
        {
            var assembly = Assembly.Load(assemblyName);
            return ReadInternal(assembly, resource);
        }

        internal Stream? ReadInternal(Assembly assembly, string resource)
        {
            if (!_assemblyNames.ContainsKey(assembly))
            {
                _assemblyNames[assembly] = assembly.GetName().Name!;
            }

            // Normalize resource path.
            var normalizedResource = resource.Replace(Path.DirectorySeparatorChar, '.')
                                             .Replace(Path.AltDirectorySeparatorChar, '.');
            // Replace consecutive periods originated from '//' with a single period.
            normalizedResource = Regex.Replace(normalizedResource, @"\.+", ".");

            return assembly.GetManifestResourceStream($"{_assemblyNames[assembly]}.{normalizedResource}");
        }
    }
}