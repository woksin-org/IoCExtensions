// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Woksin.Extensions.Tenancy;

namespace Woksin.Extensions.Configurations.Tenancy;

/// <summary>
/// Represents extension methods for adding the tenant configuration system to a host.
/// </summary>
public static class HostExtensions
{
    /// <summary>
    /// Use the tenant configuration system.
    /// </summary>
    /// <typeparam name="TTenant">The <see cref="Type"/> of the <see cref="ITenantInfo"/>.</typeparam>
    /// <param name="builder">The <see cref="IHostBuilder"/>.</param>
    /// <param name="assembly">The <see cref="Assembly"/> that will be used to find configuration classes. </param>
    /// <param name="configurationPrefixes">The configuration prefixes.</param>
    /// <returns>The builder for continuation.</returns>
    public static IHostBuilder UseTenantConfigurationExtension<TTenant>(this IHostBuilder builder, Assembly assembly, params string[] configurationPrefixes)
        where TTenant : class, ITenantInfo, new()
        => builder.ConfigureServices((_, services) =>
        {
            services
                .AddTenantConfigurationExtension<TTenant>(configurationPrefixes)
                .WithAssembly(assembly);
        });

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
                .AddTenantConfigurationExtension<TenantInfo>(configurationPrefixes)
                .WithAssembly(assembly);
        });

    /// <summary>
    /// Use the tenant configuration system.
    /// </summary>
    /// <typeparam name="TTenant">The <see cref="Type"/> of the <see cref="ITenantInfo"/>.</typeparam>
    /// <param name="builder">The <see cref="IHostBuilder"/>.</param>
    /// <param name="assembly">The <see cref="Assembly"/> that will be used to find configuration classes. </param>
    /// <param name="configure">The callback to configure the <see cref="TenantConfigurationBuilder{TTenant}"/>.</param>
    /// <param name="configurationPrefixes">The configuration prefixes.</param>
    /// <returns>The builder for continuation.</returns>
    public static IHostBuilder UseTenantConfigurationExtension<TTenant>(this IHostBuilder builder, Assembly assembly, Action<TenantConfigurationBuilder<TTenant>> configure, params string[] configurationPrefixes)
        where TTenant : class, ITenantInfo, new()
    {
        return builder.ConfigureServices((_, services) =>
        {
            var configBuilder = services
                .AddTenantConfigurationExtension<TTenant>(configurationPrefixes)
                .WithAssembly(assembly);
            configure?.Invoke(configBuilder);
        });
    }
    
    /// <summary>
    /// Use the tenant configuration system.
    /// </summary>
    /// <param name="builder">The <see cref="IHostBuilder"/>.</param>
    /// <param name="assembly">The <see cref="Assembly"/> that will be used to find configuration classes. </param>
    /// <param name="configure">The callback to configure the <see cref="TenantConfigurationBuilder{TTenant}"/>.</param>
    /// <param name="configurationPrefixes">The configuration prefixes.</param>
    /// <returns>The builder for continuation.</returns>
    public static IHostBuilder UseTenantConfigurationExtension(this IHostBuilder builder, Assembly assembly, Action<TenantConfigurationBuilder<TenantInfo>> configure, params string[] configurationPrefixes)
    {
        return builder.ConfigureServices((_, services) =>
        {
            var configBuilder = services
                .AddTenantConfigurationExtension<TenantInfo>(configurationPrefixes)
                .WithAssembly(assembly);
            configure?.Invoke(configBuilder);
        });
    }
    
    /// <summary>
    /// Use the tenant configuration system.
    /// </summary>
    /// <typeparam name="TTenant">The <see cref="Type"/> of the <see cref="ITenantInfo"/>.</typeparam>
    /// <param name="builder">The <see cref="IHostBuilder"/>.</param>
    /// <param name="configure">The callback to configure the <see cref="TenantConfigurationBuilder{TTenant}"/>.</param>
    /// <param name="configurationPrefixes">The configuration prefixes.</param>
    /// <returns>The builder for continuation.</returns>
    public static IHostBuilder UseTenantConfigurationExtension<TTenant>(this IHostBuilder builder, Action<TenantConfigurationBuilder<TTenant>> configure, params string[] configurationPrefixes)
        where TTenant : class, ITenantInfo, new()
    {
        return builder.ConfigureServices((_, services) =>
        {
            var configBuilder = services
                .AddTenantConfigurationExtension<TTenant>(configurationPrefixes);
            configure?.Invoke(configBuilder);
        });
    }

    /// <summary>
    /// Use the tenant configuration system.
    /// </summary>
    /// <param name="builder">The <see cref="IHostBuilder"/>.</param>
    /// <param name="configure">The callback to configure the <see cref="TenantConfigurationBuilder{TTenant}"/>.</param>
    /// <param name="configurationPrefixes">The configuration prefixes.</param>
    /// <returns>The builder for continuation.</returns>
    public static IHostBuilder UseTenantConfigurationExtension(this IHostBuilder builder, Action<TenantConfigurationBuilder<TenantInfo>> configure, params string[] configurationPrefixes)
    {
        return builder.ConfigureServices((_, services) =>
        {
            var configBuilder = services
                .AddTenantConfigurationExtension<TenantInfo>(configurationPrefixes);
            configure?.Invoke(configBuilder);
        });
    }

    /// <summary>
    /// Adds the tenant configuration system.
    /// </summary>
    /// <typeparam name="TTenant">The <see cref="Type"/> of the <see cref="ITenantInfo"/>.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    /// <param name="configurationPrefixes">The configuration prefixes.</param>
    /// <returns>The builder for continuation.</returns>
    /// <remarks>If UseTenantConfigurationExtension was used you should not call this.</remarks>
    public static TenantConfigurationBuilder<TTenant> AddTenantConfigurationExtension<TTenant>(this IServiceCollection services, params string[] configurationPrefixes)
        where TTenant : class, ITenantInfo, new()
        => new(services, configurationPrefixes);
}
