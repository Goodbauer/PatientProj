using Serilog;
using PatientTest.Core;
using PatientTest.Infrastructure;

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration));
// Add services to the container.


ConfigurationManager configuration = builder.Configuration;
var defualtConnection = configuration.GetConnectionString("DefaultConnection");
if (defualtConnection == null)
    throw new Exception("Set \"DefaultConnection\"");

// Core
builder.Services.AddCustomService();

// Infrastructure
builder.Services.AddContext(defualtConnection);
builder.Services.AddRepositoryInfrastructure();

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetService<Context>();
    DataSeeder.SeedData(context);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapControllers();

app.Run();