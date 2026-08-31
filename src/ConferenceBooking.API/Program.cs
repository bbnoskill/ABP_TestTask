using System.Reflection;
using Microsoft.EntityFrameworkCore;
using ConferenceBooking.Application;
using ConferenceBooking.Infrastructure;
using ConferenceBooking.Infrastructure.Data;
using ConferenceBooking.API.Middleware;

var builder = WebApplication.CreateBuilder(args);

// DI: Application + Infrastructure
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(
    builder.Configuration.GetConnectionString("DefaultConnection")
    ?? "Data Source=ConferenceBooking.db");

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new()
    {
        Title = "Conference Booking API",
        Version = "v1",
        Description = "API для управління бронюванням конференц-залів"
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
        options.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

// Auto-migrate or ensure created on startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    if (db.Database.IsRelational())
    {
        db.Database.Migrate();
    }
    else
    {
        db.Database.EnsureCreated();
    }
}

// Middleware pipeline
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();

// Потрібен для WebApplicationFactory<Program> в інтеграційних тестах
public partial class Program { }
