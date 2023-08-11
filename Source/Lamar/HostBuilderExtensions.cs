// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reflection;
using Lamar;
using IoCExtensions.Provider;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IoCExtensions.Lamar;

/// <summary>
/// Extension methods for <see cref="IHostBuilder"/>.
/// </summary>
public static class HostBuilderExtensions
{
	/// <summary>
    /// Use the Lamar IoCExtensions implementation.
	/// </summary>
	/// <param name="builder">The <see cref="IHostBuilder"/> to modify.</param>
	/// <param name="entryAssemblyName">The entry point assembly name to discover services in.</param>
	/// <param name="configureOptions">The callback for configuring <see cref="IoCExtensionsOptions"/>.</param>
	/// <param name="configureContainer">The callback for configuring <see cref="ServiceRegistry"/>.</param>
	/// <returns>The <see cref="IHostBuilder"/> for continuation.</returns>
	public static IHostBuilder UseLamarIoCExtensions(
		this IHostBuilder builder,
		string entryAssemblyName,
		Action<IoCExtensionsOptions>? configureOptions = default,
		Action<ServiceRegistry>? configureContainer = default) => UseLamarIoCExtensions(
        builder, _ => IoCExtensionsOptionsConfigurator.Configure(_, entryAssemblyName, configureOptions), configureContainer);

    /// <summary>
    /// Use the Lamar IoCExtensions implementation.
    /// </summary>
    /// <param name="builder">The <see cref="IHostBuilder"/> to modify.</param>
    /// <param name="entryAssembly">The entry point assembly to discover services in.</param>
    /// <param name="configureOptions">The callback for configuring <see cref="IoCExtensionsOptions"/>.</param>
    /// <param name="configureContainer">The callback for configuring <see cref="ServiceRegistry"/>.</param>
    /// <returns>The <see cref="IHostBuilder"/> for continuation.</returns>
    public static IHostBuilder UseLamarIoCExtensions(
        this IHostBuilder builder,
        Assembly entryAssembly,
        Action<IoCExtensionsOptions>? configureOptions = default,
        Action<ServiceRegistry>? configureContainer = default) => UseLamarIoCExtensions(
        builder, _ => IoCExtensionsOptionsConfigurator.Configure(_, entryAssembly, configureOptions), configureContainer);
    
    /// <summary>
    /// Use the Lamar IoCExtensions implementation.
    /// </summary>
    /// <param name="builder">The <see cref="IHostBuilder"/> to modify.</param>
    /// <param name="configureOptions">The callback for configuring <see cref="IoCExtensionsOptions"/>.</param>
    /// <param name="configureContainer">The callback for configuring <see cref="ServiceRegistry"/>.</param>
    /// <returns>The <see cref="IHostBuilder"/> for continuation.</returns>
    public static IHostBuilder UseLamarIoCExtensions(
        this IHostBuilder builder,
        Action<IoCExtensionsOptions>? configureOptions = default,
        Action<ServiceRegistry>? configureContainer = default) => UseLamarIoCExtensions(
        builder, _ => IoCExtensionsOptionsConfigurator.Configure(_, configureOptions), configureContainer);
    
    
    static IHostBuilder UseLamarIoCExtensions(
        IHostBuilder builder,
        Action<IServiceCollection> addIocExtensionsOptions,
        Action<ServiceRegistry>? configureContainer) =>
        builder
            .ConfigureServices((_, services) => addIocExtensionsOptions(services))
            .UseServiceProviderFactory(new ServiceProviderFactory(configureContainer));
}
