#region

using Microsoft.EntityFrameworkCore;
using RestApi.Data.Context;

#endregion

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
string conString = builder.Configuration.GetConnectionString("NameMcDatabase");
builder.Services.AddDbContext<DataContext>(it =>
        it.UseMySql(conString, ServerVersion.AutoDetect(conString))
            .UseLazyLoadingProxies()
            .UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll),
    ServiceLifetime.Singleton);

builder.Services.AddControllers();
WebApplication app = builder.Build();

// Create Database
IServiceScope scope = app.Services.CreateScope();
DataContext context = scope.ServiceProvider.GetRequiredService<DataContext>();
await context.Database.EnsureCreatedAsync();

app.UseAuthorization();

app.MapControllers();

if (!int.TryParse(app.Configuration["RunningPort"], out int port))
    return;
app.Run($"http://localhost:{port}");