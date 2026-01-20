namespace HorusV2.Core.Helpers;

public static class DateTimeHelper
{
    private const string TIME_ZONE_NAME = "E. South America Standard Time";

    public static DateTime ConvertToBrazilianTime(this DateTime dateTime)
    {
        return TimeZoneInfo.ConvertTime(dateTime, TimeZoneInfo.FindSystemTimeZoneById(TIME_ZONE_NAME));
    }
}