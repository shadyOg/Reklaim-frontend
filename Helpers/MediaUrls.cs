namespace Reklaim_frontend.Helpers;

public static class MediaUrls
{
    public static string? Resolve(string? path, string apiBase)
    {
        if (string.IsNullOrWhiteSpace(path)) return null;
        if (path.StartsWith("http", StringComparison.OrdinalIgnoreCase)) return path;
        return $"{apiBase.TrimEnd('/')}/{path.TrimStart('/')}";
    }
}
