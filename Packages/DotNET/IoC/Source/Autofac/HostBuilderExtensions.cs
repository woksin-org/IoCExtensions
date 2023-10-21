// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reflection;
using Autofac;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Woksin.Extensions.IoC.Autofac.Tenancy;
using Woksin.Extensions.IoC.Provider;
using Woksin.Extensions.IoC.Tenancy;

namespace Woksin.Extensions.IoC.Autofac;

/// <summary>
/// Extension methods for <see cref="IHostBuilder"/>.
/// </summary>
public static class HostBuilderExtensions
{
	/// <summary>
    /// Use the Autofac IoC implementation.
	/// </summary>
	/// <param name="builder">The <see cref="IHostBuilder"/> to modify.</param>
	/// <param name="entryAssemblyName">The entry point assembly name to discover services in.</param>
	/// <param name="configureOptions">The callback for configuring <see cref="IoCSettings"/>.</param>
	/// <param name="configureContainer">The callback for configuring <see cref="ContainerBuilder"/>.</param>
	/// <returns>The <see cref="IHostBuilder"/> for continuation.</returns>
	public static IHostBuilder UseAutofacIoC(
		this IHostBuilder builder,
		string entryAssemblyName,
		Action<IoCSettings>? configureOptions = default,
		Action<ContainerBuilder>? configureContainer = default) => UseAutofacIoC(
        builder, _ => IoCOptionsConfigurator.Configure(_, entryAssemblyName, configureOptions), configureContainer);

    /// <summary>
    /// Use the Autofac IoC implementation.
    /// </summary>
    /// <param name="builder">The <see cref="IHostBuilder"/> to modify.</param>
    /// <param name="entryAssembly">The entry point assembly to discover services in.</param>
    /// <param name="configureOptions">The callback for configuring <see cref="IoCSettings"/>.</param>
    /// <param name="configureContainer">The callback for configuring <see cref="ContainerBuilder"/>.</param>
    /// <returns>The <see cref="IHostBuilder"/> for continuation.</returns>
    public static IHostBuilder UseAutofacIoC(
        this IHostBuilder builder,
        Assembly entryAssembly,
        Action<IoCSettings>? configureOptions = default,
        Action<ContainerBuilder>? configureContainer = default) => UseAutofacIoC(
        builder, _ => IoCOptionsConfigurator.Configure(_, entryAssembly, configureOptions), configureContainer);

    static IHostBuilder UseAutofacIoC(
        IHostBuilder builder,
        Action<IServiceCollection> addIocExtensionsOptions,
        Action<ContainerBuilder>? configureContainer) =>
        builder
            .ConfigureServices((_, services) =>
            {
                addIocExtensionsOptions(services);
                services.AddSingleton<ICreateTenantScopedProviders, TenantScopedProviderCreator>();
            })
            .UseServiceProviderFactory(new ServiceProviderFactory(configureContainer));
}
