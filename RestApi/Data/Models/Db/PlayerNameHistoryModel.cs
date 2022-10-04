#nullable disable
using System.ComponentModel.DataAnnotations;

namespace RestApi.Data.Models.Db;

/// <summary>
/// Class that determines the database model for the database.
/// </summary>
public class PlayerNameHistoryModel
{
    /// <summary>
    /// Specify the ID
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Specify the Reference to the Player
    /// </summary>
    [Required]
    public virtual PlayerNameModel Player { get; set; }

    /// <summary>
    /// Specify the Name
    /// </summary>
    [Required]
    public string Name { get; set; }

    /// <summary>
    /// Specify the From-Date
    /// </summary>
    [Required]
    public DateTime From { get; set; }

    /// <summary>
    /// Specify the To-Date
    /// </summary>
    [Required]
    public DateTime To { get; set; }
}