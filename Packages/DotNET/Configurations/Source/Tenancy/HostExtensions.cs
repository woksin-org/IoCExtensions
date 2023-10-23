// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Woksin.Extensions.IoC;
using Woksin.Extensions.IoC.Tenancy;

namespace Woksin.Extensions.Configurations.Tenancy;

/// <summary>
/// Represents extension methods for adding the configuration system to a host.
/// </summary>
public static class HostExtensions
{
	/// <summary>
	/// Use the configuration system.
	/// </summary>
	/// <param name="builder">The <see cref="IHostBuilder"/>.</param>
	/// <param name="configurationPrefixes">The configuration prefixes.</param>
	/// <returns>The builder for continuation.</returns>
	public static IHostBuilder UseConfigurationExtension(this IHostBuilder builder, params string[] configurationPrefixes)
        => builder.ConfigureServices((_, services) => services.AddConfigurationExtension(configurationPrefixes));

    /// <summary>
    /// Adds the configuration system.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    /// <param name="configurationPrefixes">The configuration prefixes.</param>
    /// <returns>The builder for continuation.</returns>
    public static IServiceCollection AddConfigurationExtension(this IServiceCollection services, params string[] configurationPrefixes)
    {
        services.Configure<IoCSettings>(settings => settings.AdditionalAssemblies.Add(typeof(HostExtensions).Assembly));
		AddConfigurationPrefix(services, configurationPrefixes);
		services.Add(ServiceDescriptor.Singleton(typeof(IOptionsFactory<>), typeof(RootContainerConfigurationsExtensionOptionsFactory<>)));
        services.AddTenantScopedServices(tenantServices =>
        {
            tenantServices.Add(ServiceDescriptor.Singleton(typeof(IOptions<>), typeof(UnnamedOptionsManager<>)));
            tenantServices.Add(ServiceDescriptor.Scoped(typeof(IOptionsSnapshot<>), typeof(OptionsManager<>)));
            tenantServices.Add(ServiceDescriptor.Singleton(typeof(IOptionsMonitor<>), typeof(OptionsMonitor<>)));
            tenantServices.Add(ServiceDescriptor.Transient(typeof(IOptionsFactory<>), typeof(ConfigurationsExtensionOptionsFactory<>)));
            tenantServices.Add(ServiceDescriptor.Singleton(typeof(IOptionsMonitorCache<>), typeof(OptionsCache<>)));
        });
        return services;
	}

    static void AddConfigurationPrefix(IServiceCollection serviceCollection, string[] configurationPrefixes)
    {
        var prefix = ConfigurationPath.Combine(configurationPrefixes);
        if (!string.IsNullOrEmpty(prefix))
        {
            prefix += ConfigurationPath.KeyDelimiter;
        }

        serviceCollection.TryAddSingleton(new ConfigurationPrefix(prefix));
    }
}
