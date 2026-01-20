using System.Text.Json;

namespace HorusV2.Core.Helpers;

public static class JsonHelper
{
    private static readonly JsonSerializerOptions Options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public static string ToJson<TObject>(TObject @object)
    {
        return JsonSerializer.Serialize(@object, Options);
    }

    public static TObject? FromJson<TObject>(string json)
    {
        return JsonSerializer.Deserialize<TObject>(json, Options);
    }

    public static TTarget? SerializeAndDeserialize<TOrigin, TTarget>(TOrigin @object)
    {
        string json = ToJson(@object);

        return FromJson<TTarget>(json);
    }
}