using Microsoft.AspNetCore.Mvc;
using Samples.Tenancy;
using Woksin.Extensions.Configurations.Tenancy;
using Woksin.Extensions.IoC.Microsoft;
using Woksin.Extensions.IoC.Tenancy.Middleware;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseMicrosoftIoC("Samples.Tenancy");
builder.Host.UseConfigurationExtension();
builder.Services.AddSwaggerGen(options =>
{
    options.OperationFilter<TenantIdHeaderFilter>();
});
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});
app.UseTenantIdStrategies();

app.MapGet("/tenantSingleton", ([FromServices] TenantSingleton tenantSingleton) => tenantSingleton._tenantConfig);

app.Run();
