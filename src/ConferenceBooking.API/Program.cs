using ConferenceBooking.Application;
using ConferenceBooking.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// DI: Application + Infrastructure layers
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(
    builder.Configuration.GetConnectionString("DefaultConnection")
    ?? "Data Source=ConferenceBooking.db");

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
