#nullable disable
#region

using System.Net;

#endregion

namespace RestApi.Data.Models;

public class LogModel
{
    public int Id { get; set; }
    public string Message { get; set; }
    public DateTime Date { get; set; } = DateTime.UtcNow;
    public IPAddress? IpAddress { get; set; }
}