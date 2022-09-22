#region

using System.Reflection;
using McNamesApi.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

#endregion

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var conString = builder.Configuration.GetConnectionString("NameMcDatabase");
builder.Services.AddDbContext<DataContext>(it =>
    it.UseMySql(conString, ServerVersion.AutoDetect(conString))
        .UseLazyLoadingProxies());

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
            Title = "McNames API",
            Description = "Provide UUID->Name, Name->UUID and NameHistory",
            Contact = new OpenApiContact
            {
                Email = "felix.hake@saveapis.com",
                Name = "Felix Hake",
                Url = new Uri("https://saveapis.com")
            },
            Version = "v1"
        });
        var filePath = Path.Combine(AppContext.BaseDirectory, "McNamesApi.xml");
        c.IncludeXmlComments(filePath);
    }
);

var app = builder.Build();

// Create Database
var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<DataContext>();
await context.Database.EnsureCreatedAsync();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();