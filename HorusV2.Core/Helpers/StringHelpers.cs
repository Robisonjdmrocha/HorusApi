namespace HorusV2.Core.Helpers;

public static class StringHelpers
{
    public static string GetNumbers(this string source)
    {
        return new string(source.Where(char.IsDigit).ToArray());
    }
}