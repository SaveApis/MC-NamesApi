#nullable disable

#region

using System.ComponentModel.DataAnnotations;

#endregion

namespace McNamesApi.Data.Models;

/// <summary>
///     Model for the NameHistory
/// </summary>
public class NameHistoryModel
{
    /// <summary>
    ///     Specifies the ID of the NameHistory
    /// </summary>
    [Required]
    public int Id { get; set; }

    /// <summary>
    ///     Specifies the relation to the player.
    /// </summary>
    [Required]
    public virtual PlayerModel Player { get; set; }

    /// <summary>
    ///     Specifies the name of the NameHistory
    /// </summary>
    [Required]
    public string Name { get; set; }

    /// <summary>
    ///     Specifies the 'from' time of the NameHistory
    /// </summary>
    [Required]
    public DateTime Since { get; set; } = DateTime.UtcNow;

    /// <summary>
    ///     Specifies the 'to' time of the NameHistory
    /// </summary>
    [Required]
    public DateTime To { get; set; } = DateTime.UtcNow;
}