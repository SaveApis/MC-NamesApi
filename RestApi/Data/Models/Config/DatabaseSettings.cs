namespace RestApi.Data.Models.Config;

public class DatabaseSettings
{
    public string Host { get; set; } = string.Empty;
    public string Database { get; set; } = string.Empty;
    public uint Port { get; set; } = 3306;
    public string User { get; set; } = string.Empty;
    public string Password { get; set; }
}