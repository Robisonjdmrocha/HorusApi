using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace HorusV2.API.Configuration;

public static class DotEnvConfiguration
{
    public static IConfigurationBuilder AddDotEnvFile(
        this IConfigurationBuilder builder,
        string path,
        bool optional = true)
    {
        if (string.IsNullOrWhiteSpace(path))
            return builder;

        if (!File.Exists(path))
        {
            if (optional)
                return builder;

            throw new FileNotFoundException("Missing required .env file.", path);
        }

        Dictionary<string, string?> values = Parse(File.ReadAllLines(path));
        return builder.AddInMemoryCollection(values);
    }

    private static Dictionary<string, string?> Parse(IEnumerable<string> lines)
    {
        var values = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase);

        foreach (var rawLine in lines)
        {
            var line = rawLine.Trim();
            if (line.Length == 0 || line.StartsWith('#'))
                continue;

            if (line.StartsWith("export ", StringComparison.OrdinalIgnoreCase))
                line = line["export ".Length..].TrimStart();

            var idx = line.IndexOf('=');
            if (idx <= 0)
                continue;

            var key = line[..idx].Trim();
            var value = line[(idx + 1)..].Trim();

            if (value.Length >= 2 &&
                ((value.StartsWith('"') && value.EndsWith('"')) || (value.StartsWith('\'') && value.EndsWith('\''))))
            {
                value = value[1..^1];
            }

            key = key.Replace("__", ":", StringComparison.Ordinal);
            values[key] = value;
        }

        return values;
    }
}
