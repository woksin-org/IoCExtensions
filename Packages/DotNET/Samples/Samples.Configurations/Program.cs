using Microsoft.Extensions.Options;
using Samples.Configurations;
using Woksin.Extensions.Configurations;
using Woksin.Extensions.Configurations.Tenancy;
using Woksin.Extensions.Tenancy;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseTenantConfigurationExtension(typeof(Config).Assembly, _ =>
{
    _.ConfigureTenancy(configurationPathParts: "Tenancy");
    
});
var app = builder.Build();
var options = app.Services.GetRequiredService<IOptions<Config>>();
var tenantOptions = app.Services.GetRequiredService<IOptions<TenantConfig>>();
var tenancyOptions = app.Services.GetRequiredService<IOptions<TenancyOptions<TenantInfo>>>();
var monitor = app.Services.GetRequiredService<IOptionsMonitor<Config>>();
monitor.OnChange(config =>
{
    Console.WriteLine(config.Value);
});
app.MapGet("/", () => "Hello World!");

app.Run();
