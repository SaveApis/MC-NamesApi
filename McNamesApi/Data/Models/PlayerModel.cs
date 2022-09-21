#nullable disable

#region

using System.ComponentModel.DataAnnotations;

#endregion

namespace McNamesApi.Data.Models;

/// <summary>
///     Model for the Player
/// </summary>
public class PlayerModel
{
    /// <summary>
    ///     Specifies the player’s UUID. (Is the primary key in the database)
    /// </summary>
    [Key]
    [Required]
    public Guid Uuid { get; set; }

    /// <summary>
    ///     Specifies the name of the player.
    /// </summary>
    [Required]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    ///     Specifies the “Created” time of the player. (Displays when the player was entered in the database)
    /// </summary>
    public DateTime Created { get; set; } = DateTime.UtcNow;

    /// <summary>
    ///     Specifies the “Last Change” time of the player.
    /// </summary>
    public DateTime LastChange { get; set; } = DateTime.UtcNow;
}

/// <summary>
///     Model for the AddDto
/// </summary>
public class PlayerModelAddDto
{
    /// <summary>
    ///     Specifies the UUID of the Player
    /// </summary>
    public Guid Uuid { get; set; }

    /// <summary>
    ///     Specifies the Name of the Player
    /// </summary>
    public string Name { get; set; }
}