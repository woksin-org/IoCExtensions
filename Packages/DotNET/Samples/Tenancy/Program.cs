using Samples.Tenancy;
using Woksin.Extensions.Configurations.Tenancy;
using Woksin.Extensions.IoC.Autofac;
using Woksin.Extensions.IoC.Microsoft;
using Woksin.Extensions.IoC.Tenancy;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseAutofacIoC("Samples.Tenancy");
// builder.Host.UseMicrosoftIoC("Samples.Tenancy");
builder.Host.UseConfigurationExtension();
var app = builder.Build();

var provider = app.Services.GetRequiredService<ITenantScopedServiceProviders>().ForTenant("some-tenant");
var tenantSingleton = provider.GetRequiredService<TenantSingleton>();
var singleton = app.Services.GetRequiredService<Singleton>();
var lazySingleton = app.Services.GetService<Lazy<Singleton>>();
var lazyTenantSingleton = provider.GetService<Lazy<TenantSingleton>>();
app.Run();
