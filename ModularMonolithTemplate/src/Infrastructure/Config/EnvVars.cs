namespace ModularMonolithTemplate.Infrastructure.Config;

public static class EnvVars
{
    public static void Load(string? basePath = null)
    {
        try
        {
            if (!string.IsNullOrWhiteSpace(basePath))
            {
                DotNetEnv.Env.Load(System.IO.Path.Combine(basePath!, ".env"));
                return;
            }
        }
        catch { /* ignore and try default */ }

        try { DotNetEnv.Env.Load(); }
        catch { /* ignore if file not found */ }
    }

    public static string? Get(string key, string? defaultValue = null)
    {
        var value = Environment.GetEnvironmentVariable(key);
        return string.IsNullOrWhiteSpace(value) ? defaultValue : value;
    }

    public static bool GetBool(string key, bool defaultValue = false)
    {
        var v = Get(key);
        if (string.IsNullOrWhiteSpace(v)) return defaultValue;
        return v.Trim().ToLowerInvariant() switch
        {
            "1" or "true" or "yes" or "y" => true,
            "0" or "false" or "no" or "n" => false,
            _ => defaultValue
        };
    }

    public static int GetInt(string key, int defaultValue = 0)
    {
        var v = Get(key);
        return int.TryParse(v, out var i) ? i : defaultValue;
    }
}
