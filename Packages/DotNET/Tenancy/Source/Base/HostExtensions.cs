// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Woksin.Extensions.Tenancy;

/// <summary>
/// Represents extension methods for adding the configuration system to a host.
/// </summary>
public static class HostExtensions
{
	public static IHostBuilder UseTenancyExtension<TTenant>(this IHostBuilder builder, Action<TenancyBuilder<TTenant>>? configureTenancy = null)
        where TTenant : class, ITenantInfo, new()
    {
        return builder.ConfigureServices((_, services) =>
        {
            var tenancyBuilder = services.AddTenancyExtension<TTenant>();
            configureTenancy?.Invoke(tenancyBuilder);
            tenancyBuilder.WithDefaultStrategies();
        });
    }

    public static IHostBuilder UseTenancyExtension(this IHostBuilder builder, Action<TenancyBuilder<TenantInfo>>? configureTenancy = null)
    {
        return builder.ConfigureServices((_, services) =>
        {
            var tenancyBuilder = services.AddTenancyExtension<TenantInfo>();
            configureTenancy?.Invoke(tenancyBuilder);
            tenancyBuilder.WithDefaultStrategies();
        });
    }

    public static TenancyBuilder<TTenant> AddTenancyExtension<TTenant>(this IServiceCollection services) where TTenant : class, ITenantInfo, new()
        => new(services);
    
    public static TenancyBuilder<TenantInfo> AddTenancyExtension(this IServiceCollection services)
        => new(services);
}
