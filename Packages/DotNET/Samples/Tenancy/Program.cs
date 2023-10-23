using Samples.Tenancy;
using Woksin.Extensions.Configurations.Tenancy;
using Woksin.Extensions.IoC.Microsoft;
using Woksin.Extensions.IoC.Tenancy;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseMicrosoftIoC("Samples.Tenancy");
builder.Host.UseConfigurationExtension();
var app = builder.Build();

var provider = app.Services.GetRequiredService<ITenantScopedServiceProviders>().ForTenant("some-tenant");
provider.GetRequiredService<TenantSingleton>();
app.Services.GetRequiredService<Singleton>();
app.Run();
