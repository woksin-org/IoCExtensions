using Microsoft.Extensions.Options;
using Samples.Configurations;
using Woksin.Extensions.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseConfigurationExtension(typeof(Config).Assembly);
var app = builder.Build();
var options = app.Services.GetRequiredService<IOptions<Config>>();
var monitor = app.Services.GetRequiredService<IOptionsMonitor<Config>>();
monitor.OnChange(config =>
{
    Console.WriteLine(config.Value);
});
app.MapGet("/", () => "Hello World!");

app.Run();
