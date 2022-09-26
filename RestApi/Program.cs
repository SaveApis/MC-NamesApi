#region

using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using RestApi.Data.Context;
using Swashbuckle.AspNetCore.Filters;

#endregion

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var conString = builder.Configuration.GetConnectionString("NameMcDatabase");
builder.Services.AddDbContext<DataContext>(it =>
        it.UseMySql(conString, ServerVersion.AutoDetect(conString))
            .UseLazyLoadingProxies()
            .UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll),
    ServiceLifetime.Singleton);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddSwaggerExamplesFromAssemblies(Assembly.GetEntryAssembly());
builder.Services.AddSwaggerGen(
    c =>
    {
        c.ExampleFilters();
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "McNames API v1",
            Description = "Provide UUID->Name, Name->UUID and NameHistory",
            Contact = new OpenApiContact
            {
                Email = "felix.hake@saveapis.com",
                Name = "Felix Hake",
                Url = new Uri("https://saveapis.com")
            },
            Version = "v1",
            License = new OpenApiLicense()
            {
                Name = "Apache Licence 2.0",
                Url = new Uri("https://raw.githubusercontent.com/SaveApis/MC-NamesApi/main/LICENCE")
            }
        });

        var filePath = Path.Combine(AppContext.BaseDirectory, "RestApi.xml");
        c.IncludeXmlComments(filePath);
    }
);
builder.Services.AddMvcCore().AddApiExplorer();
var app = builder.Build();

// Create Database
var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<DataContext>();
await context.Database.EnsureCreatedAsync();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();