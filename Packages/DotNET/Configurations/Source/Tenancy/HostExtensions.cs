// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Woksin.Extensions.Configurations.Tenancy;

/// <summary>
/// Represents extension methods for adding the tenant configuration system to a host.
/// </summary>
public static class HostExtensions
{
    /// <summary>
    /// Use the tenant configuration system.
    /// </summary>
    /// <param name="builder">The <see cref="IHostBuilder"/>.</param>
    /// <param name="assembly">The <see cref="Assembly"/> that will be used to find configuration classes. </param>
    /// <param name="configurationPrefixes">The configuration prefixes.</param>
    /// <returns>The builder for continuation.</returns>
    public static IHostBuilder UseTenantConfigurationExtension(this IHostBuilder builder, Assembly assembly, params string[] configurationPrefixes)
        => builder.ConfigureServices((_, services) =>
        {
            services
                .AddTenantConfigurationExtension(configurationPrefixes)
                .WithAssembly(assembly);
        });

    /// <summary>
    /// Use the tenant configuration system.
    /// </summary>
    /// <param name="builder">The <see cref="IHostBuilder"/>.</param>
    /// <param name="assembly">The <see cref="Assembly"/> that will be used to find configuration classes. </param>
    /// <param name="configure">The callback to configure the <see cref="TenantConfigurationExtensionBuilder"/>.</param>
    /// <param name="configurationPrefixes">The configuration prefixes.</param>
    /// <returns>The builder for continuation.</returns>
    public static IHostBuilder UseTenantConfigurationExtension(this IHostBuilder builder, Assembly assembly, Action<TenantConfigurationExtensionBuilder> configure, params string[] configurationPrefixes)
    {
        return builder.ConfigureServices((_, services) =>
        {
            var configBuilder = services
                .AddTenantConfigurationExtension(configurationPrefixes)
                .WithAssembly(assembly);
            configure?.Invoke(configBuilder);
        });
    }
    /// <summary>
    /// Use the tenant configuration system.
    /// </summary>
    /// <param name="builder">The <see cref="IHostBuilder"/>.</param>
    /// <param name="configure">The callback to configure the <see cref="TenantConfigurationExtensionBuilder"/>.</param>
    /// <param name="configurationPrefixes">The configuration prefixes.</param>
    /// <returns>The builder for continuation.</returns>
    public static IHostBuilder UseTenantConfigurationExtension(this IHostBuilder builder, Action<TenantConfigurationExtensionBuilder> configure, params string[] configurationPrefixes)
    {
        return builder.ConfigureServices((_, services) =>
        {
            var configBuilder = services
                .AddTenantConfigurationExtension(configurationPrefixes);
            configure?.Invoke(configBuilder);
        });
    }

    /// <summary>
    /// Adds the tenant configuration system.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    /// <param name="configurationPrefixes">The configuration prefixes.</param>
    /// <returns>The builder for continuation.</returns>
    /// <remarks>If UseTenantConfigurationExtension was used you should not call this.</remarks>
    public static TenantConfigurationExtensionBuilder AddTenantConfigurationExtension(this IServiceCollection services, params string[] configurationPrefixes)
        => new(services, configurationPrefixes);
}
