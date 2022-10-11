namespace RestApi.Data.Models.Config;

public class DatabaseSettings
{
    public string Host { get; set; }
    public string Database { get; set; }
    public uint Port { get; set; }
    public string User { get; set; }
    public string Password { get; set; }
}