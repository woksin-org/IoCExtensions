// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Woksin.Extensions.Configurations;
using Woksin.Extensions.IoC;
using Woksin.Extensions.IoC.Tenancy;
using Woksin.Extensions.IoC.Tenancy.Middleware;

namespace Samples.Tenancy;

public class TenantIdHeaderFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        operation.Parameters ??= new List<OpenApiParameter>();

        operation.Parameters.Add(new OpenApiParameter{Name = TenantIdFromHeaderStrategy.DefaultTenantIdHeader, In = ParameterLocation.Header, Required = false});
    }
}

public abstract class TenantService
{
    protected TenantService(TenantId tenant)
    {
        Console.WriteLine($"Created {GetType()} with tenant id {tenant}");
    }
}

public abstract class Service
{
    protected Service()
    {
        Console.WriteLine($"Created {GetType()}");
    }
}


[PerTenant, WithLifetime(ServiceLifetime.Singleton), RegisterAsSelf]
public class TenantSingleton : TenantService
{
    public TenantConfig _tenantConfig; 
    public TenantSingleton(TenantId tenant, IOptions<TenantConfig> tenantConfig, IOptions<Config> config, IEnumerable<IEnumerableService> services, IOptionsMonitor<TenantConfig> tenantConfigMoniitor) : base(tenant)
    {
        _tenantConfig = tenantConfigMoniitor.CurrentValue;
        tenantConfigMoniitor.OnChange(config =>
        {
            _tenantConfig = config;
            Console.WriteLine($"New tenant config value {config.Value}");
        });
    }
}

[WithLifetime(ServiceLifetime.Singleton), RegisterAsSelf]
public class Singleton : Service
{
    Config _config; 
    public Singleton(IOptions<Config> config, IEnumerable<IEnumerableService> services, IOptionsMonitor<Config> configMoniitor) : base()
    {
        _config = configMoniitor.CurrentValue;
        configMoniitor.OnChange(config =>
        {
            _config = config;
            
            Console.WriteLine($"New config value {config.Value}");
        });
    }
}

public interface IEnumerableService
{ }

[WithLifetime(ServiceLifetime.Singleton)]
public class GlobalEnumerableService : IEnumerableService
{
    
}
[PerTenant, WithLifetime(ServiceLifetime.Singleton)]
public class TenantEnumerableService : TenantService, IEnumerableService
{
    public TenantEnumerableService(TenantId tenant) : base(tenant)
    {
    }
}

[Configuration("Config")]
public class Config
{
    public string Value { get; set; }
}
[PerTenant, Configuration("TenantConfig")]
public class TenantConfig
{
    public string Value { get; set; }
}
