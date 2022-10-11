#region

using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using RestApi.Data.Context;
using RestApi.Data.Models.Config;

#endregion

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);


ApiSettings apiSettings = builder.Configuration.GetSection("ApiSettings").Get<ApiSettings>();
DatabaseSettings databaseSettings = builder.Configuration.GetSection("DatabaseSettings").Get<DatabaseSettings>();


// Add services to the container.
MySqlConnectionStringBuilder conBuilder = new MySqlConnectionStringBuilder()
{
    Server = databaseSettings.Host,
    Database = databaseSettings.Database,
    UserID = databaseSettings.User,
    Password = databaseSettings.Password,
    Port = databaseSettings.Port,
    Pooling = true,
    ApplicationName = "McNames REST-Api",
    AllowUserVariables = true
};
builder.Services.AddDbContext<DataContext>(it =>
        it.UseMySql(conBuilder.ToString(), ServerVersion.AutoDetect(conBuilder.ToString()))
            .UseLazyLoadingProxies(),
    ServiceLifetime.Singleton);

builder.Services.AddControllers();

if (apiSettings.SwaggerEnabled)
    builder.Services.AddSwaggerGen();
WebApplication app = builder.Build();

// Create Database
IServiceScope scope = app.Services.CreateScope();
DataContext context = scope.ServiceProvider.GetRequiredService<DataContext>();
await context.Database.EnsureCreatedAsync();

app.UseAuthorization();

if (apiSettings.SwaggerEnabled)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.Run($"http://localhost:{apiSettings.RunningPort}");