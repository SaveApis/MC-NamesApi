#nullable disable

#region

using Microsoft.EntityFrameworkCore;
using RestApi.Data.Models.Db;

#endregion

namespace RestApi.Data.Context;

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
    /// Ref DbSet to EF-Core
    /// </summary>
    public DbSet<AgreementModel> Agreements { get; set; }

    /// <summary>
    /// Ref DbSet to EF-Core
    /// </summary>
    public DbSet<PlayerNameModel> Names { get; set; }

    /// <summary>
    /// Ref DbSet to EF-Core
    /// </summary>
    public DbSet<PlayerNameHistoryModel> Histories { get; set; }

    /// <summary>
    /// Ref DbSet to EF-Core
    /// </summary>
    public DbSet<LogModel> Logs { get; set; }
}