#nullable disable

#region

using System.Net;

#endregion

namespace RestApi.Data.Models.Db;

/// <summary>
/// Class that determines the database model for the database.
/// </summary>
public class LogModel
{
    /// <summary>
    /// Specifies the ID.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Specify the Message.
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// Specify the Date of the Entry.
    /// </summary>
    public DateTime Date { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Specify the Address.
    /// </summary>
    public IPAddress IpAddress { get; set; }
}