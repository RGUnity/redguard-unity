using System.IO;

/// <summary>
/// Shared path resolution and name normalization utilities for FFI loaders.
/// </summary>
public static class FFIPathUtils
{
    /// <summary>
    /// Resolves a case-insensitive file path by trying the uppercase extension first,
    /// then the lowercase variant. Returns null if neither exists.
    /// </summary>
    public static string ResolveFile(string folder, string name, string extension)
    {
        string upper = Path.Combine(folder, name + extension);
        if (File.Exists(upper))
        {
            return upper;
        }

        string lower = Path.Combine(folder, name.ToLowerInvariant() + extension.ToLowerInvariant());
        if (File.Exists(lower))
        {
            return lower;
        }

        return null;
    }

    /// <summary>
    /// Strips directory and extension from a model path, returning the uppercase stem.
    /// e.g. "ART\models\CYRUS.3DC" → "CYRUS"
    /// </summary>
    public static string NormalizeModelName(string modelName)
    {
        if (string.IsNullOrEmpty(modelName))
        {
            return string.Empty;
        }

        string normalized = modelName.Replace('\\', '/');
        int slash = normalized.LastIndexOf('/');
        string file = slash >= 0 ? normalized.Substring(slash + 1) : normalized;
        int dot = file.LastIndexOf('.');
        string stem = dot > 0 ? file.Substring(0, dot) : file;
        return stem.ToUpperInvariant();
    }
}
