#nullable disable

#region

using McNamesApi.Data.Models;
using Microsoft.EntityFrameworkCore;

#endregion

namespace McNamesApi.Data.Context;

/// <summary>
///     Ref Class to EF-Core
/// </summary>
public class DataContext : DbContext
{
    /// <summary>
    ///     Ref Constructor to EF-Core
    /// </summary>
    /// <param name="options"></param>
    public DataContext(DbContextOptions options) : base(options)
    {
    }

    /// <summary>
    ///     Ref to the Players-Table
    /// </summary>
    public DbSet<PlayerModel> Players { get; set; }

    /// <summary>
    ///     Ref to the NameHistory-Table
    /// </summary>
    public DbSet<NameHistoryModel> NameHistory { get; set; }
}