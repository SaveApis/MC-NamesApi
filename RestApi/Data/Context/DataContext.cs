#nullable disable

#region

using Microsoft.EntityFrameworkCore;
using RestApi.Data.Models;

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

    public DbSet<AgreementModel> Agreements { get; set; }
    public DbSet<PlayerNameModel> PlayerNames { get; set; }
    public DbSet<PlayerNameHistoryModel> PlayerNameHistories { get; set; }
    public DbSet<LogModel> Logs { get; set; }
}