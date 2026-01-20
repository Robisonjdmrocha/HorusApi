namespace HorusV2.Application.Settings;

internal sealed class RelationalDatabaseSettings
{
    public RelationalDatabaseSettings()
    {
    }

    public RelationalDatabaseSettings(string connectionString)
    {
        ConnectionString = connectionString;
    }

    public string ConnectionString { get; set; }
}