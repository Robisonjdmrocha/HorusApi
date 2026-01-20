namespace HorusV2.Core.Helpers;

public static class EnumerableExtensions
{
    public static void ForEach<TObject>(this IEnumerable<TObject> source, Action<TObject> action)
    {
        foreach (TObject @object in source) action(@object);
    }
}