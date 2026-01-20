namespace HorusV2.HorusIntegration.Settings;

public class HorusIntegrationSettings
{
    public HorusIntegrationSettings()
    {
    }

    public HorusIntegrationSettings(string baseUri, string authUri, string userAccess, string password)
    {
        BaseUri = baseUri;
        AuthUri = authUri;
        UserAccess = userAccess;
        Password = password;
    }

    public string BaseUri { get; set; }
    public string AuthUri { get; set; }
    public string UserAccess { get; set; }
    public string Password { get; set; }
}