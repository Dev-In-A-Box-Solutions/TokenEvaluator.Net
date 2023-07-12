using System;
using System.Collections.Generic;
using System.Reflection;

namespace TokenEvaluator.Net.Utils
{
    internal class AssemblyComparer : IEqualityComparer<Assembly>
    {
        public bool Equals(Assembly x, Assembly y)
        {
            return StringComparer.OrdinalIgnoreCase.Equals(x?.FullName, y?.FullName);
        }

        public int GetHashCode(Assembly obj)
        {
            return StringComparer.OrdinalIgnoreCase.GetHashCode(obj.FullName);
        }
    }
}