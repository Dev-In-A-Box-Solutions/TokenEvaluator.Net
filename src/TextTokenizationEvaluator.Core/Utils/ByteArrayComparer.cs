namespace TextTokenizationEvaluator.Core.EncodingUtils;

internal class ByteArrayComparer : IEqualityComparer<byte[]>
{
    /// <summary>
    /// Compares two byte arrays for equality. Two byte arrays are considered equal if they are of the same length and all corresponding elements are equal.
    /// </summary>
    /// <param name="x">The first byte array to compare.</param>
    /// <param name="y">The second byte array to compare.</param>
    /// <returns>True if the two byte arrays are equal, false otherwise.</returns>
    public bool Equals(byte[]? x, byte[]? y)
    {
        if (ReferenceEquals(x, y))
        {
            return true;
        }

        if (x == null || y == null)
        {
            return false;
        }

        if (x.Length != y.Length)
        {
            return false;
        }

        for (int i = 0; i < x.Length; i++)
        {
            if (x[i] != y[i])
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// Returns a hash code for the specified byte array.
    /// </summary>
    /// <param name="obj">The byte array for which a hash code is to be returned.</param>
    /// <returns>A hash code for the specified byte array.</returns>
    /// <exception cref="ArgumentNullException">The byte array is null.</exception>
    public int GetHashCode(byte[]? obj)
    {
        if (obj == null)
        {
            throw new ArgumentNullException(nameof(obj));
        }

        unchecked
        {
            int hash = 17;
            foreach (byte b in obj)
            {
                hash = hash * 31 + b;
            }
            return hash;
        }
    }
}