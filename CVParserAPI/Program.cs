using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using CVParserAPI.Services;
using CVParserAPI.Services.Interfaces;
using Serilog.Sinks.Grafana.Loki;

var builder = WebApplication.CreateBuilder(args);

// Configurar Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .Enrich.WithProperty("Application", "CVParserAPI")
    .WriteTo.Console()
    .WriteTo.File("logs/cvparser-.txt", rollingInterval: RollingInterval.Day)
    .WriteTo.GrafanaLoki(
        "http://loki:3100",
        new[] { new LokiLabel { Key = "app", Value = "cvparserapi" } },
        credentials: null,
        propertiesAsLabels: new[] { "level", "Application" },
        batchPostingLimit: 100,
        queueLimit: 100000,
        period: TimeSpan.FromSeconds(2)
    )
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Registrar servicios
builder.Services.AddScoped<IOcrService, MockOcrService>();
builder.Services.AddScoped<ILlmService, MockLlmService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

try
{
    Log.Information("Iniciando la aplicación CVParser API");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "La aplicación se detuvo inesperadamente");
}
finally
{
    Log.CloseAndFlush();
}
