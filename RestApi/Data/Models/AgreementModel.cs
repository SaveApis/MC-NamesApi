#nullable disable

#region

using System.ComponentModel.DataAnnotations;

#endregion

namespace RestApi.Data.Models;

/// <summary>
/// Class that determines the database model for the database.
/// </summary>
public class AgreementModel
{
    /// <summary>
    /// The UUID of the Player
    /// </summary>
    [Key]
    [Required]
    public Guid Uuid { get; set; } = Guid.NewGuid();

    /// <summary>
    /// The value of whether the player has given his consent.
    /// </summary>
    [Required]
    public bool AgreeValue { get; set; } = false;
}