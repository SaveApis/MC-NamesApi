#nullable disable
using System.ComponentModel.DataAnnotations;

namespace RestApi.Data.Models.Db;

/// <summary>
/// Class that determines the database model for the database.
/// </summary>
public class PlayerNameModel
{
    /// <summary>
    /// Specify the UUID
    /// </summary>
    [Key] [Required] public Guid Uuid { get; set; }

    /// <summary>
    /// Specify the Name
    /// </summary>
    [Required] public string Name { get; set; }

    /// <summary>
    /// Specify the Since-Date
    /// </summary>
    [Required]
    public DateTime Since { get; set; }
}