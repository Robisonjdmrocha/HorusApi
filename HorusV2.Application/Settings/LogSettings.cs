namespace HorusV2.Application.Settings;

public class LogSettings
{
    public LogSettings()
    {
    }

    public LogSettings(string logTextFilePath)
    {
        LogTextFilePath = logTextFilePath;
    }

    public string LogTextFilePath { get; set; }
}

public class HorusSettings
{
    public int IbgeCode { get; set; } = 0;
    public int UserId { get; set; } = 0;
    public string BaseUri { get; set; } = string.Empty;
    public string AuthUri { get; set; } = string.Empty;
    public string UserAccess { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}