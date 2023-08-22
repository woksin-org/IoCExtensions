// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Woksin.Extensions.IoC.Provider;

namespace Woksin.Extensions.IoC.Microsoft;

/// <summary>
/// Extension methods for <see cref="IHostBuilder"/>.
/// </summary>
public static class HostBuilderExtensions
{
	/// <summary>
	/// Use the Microsoft IoC implementation.
	/// </summary>
	/// <param name="builder">The <see cref="IHostBuilder"/>.</param>
	/// <param name="entryAssemblyName">The entry point assembly name to discover services in.</param>
	/// <param name="configureOptions">The callback for configuring <see cref="IoCSettings"/>.</param>
	/// <param name="configureContainer">The callback for configuring <see cref="IServiceCollection"/>.</param>
	/// <returns>The <see cref="IHostBuilder"/> for continuation.</returns>
	public static IHostBuilder UseMicrosoftIoC(
	    this IHostBuilder builder,
		string entryAssemblyName,
	    Action<IoCSettings>? configureOptions = default,
	    Action<IServiceCollection>? configureContainer = default) =>
        UseMicrosoftIoC(builder, _ => IoCOptionsConfigurator.Configure(_,entryAssemblyName, configureOptions), configureContainer);

	/// <summary>
    /// Use the Microsoft IoC implementation.
    /// </summary>
    /// <param name="builder">The <see cref="IHostBuilder"/> to modify.</param>
    /// <param name="entryAssembly">The entry point assembly to discover services in.</param>
    /// <param name="configureOptions">The callback for configuring <see cref="IoCSettings"/>.</param>
    /// <param name="configureContainer">The callback for configuring <see cref="IServiceCollection"/>.</param>
    /// <returns>The <see cref="IHostBuilder"/> for continuation.</returns>
    public static IHostBuilder UseMicrosoftIoC(
		this IHostBuilder builder,
		Assembly entryAssembly,
		Action<IoCSettings>? configureOptions = default,
		Action<IServiceCollection>? configureContainer = default) =>
        UseMicrosoftIoC(builder, _ => IoCOptionsConfigurator.Configure(_, entryAssembly, configureOptions), configureContainer);
    
    /// <summary>
    /// Use the Microsoft IoC implementation.
    /// </summary>
    /// <param name="builder">The <see cref="IHostBuilder"/> to modify.</param>
    /// <param name="configureOptions">The callback for configuring <see cref="IoCSettings"/>.</param>
    /// <param name="configureContainer">The callback for configuring <see cref="IServiceCollection"/>.</param>
    /// <returns>The <see cref="IHostBuilder"/> for continuation.</returns>
    public static IHostBuilder UseMicrosoftIoC(
        this IHostBuilder builder,
        Action<IoCSettings>? configureOptions = default,
        Action<IServiceCollection>? configureContainer = default) =>
        UseMicrosoftIoC(builder, _ => IoCOptionsConfigurator.Configure(_, configureOptions), configureContainer);
    
    static IHostBuilder UseMicrosoftIoC(
        IHostBuilder builder,
        Action<IServiceCollection> addIocExtensionsOptions,
        Action<IServiceCollection>? configureContainer) =>
        builder
            .ConfigureServices((_, services) => addIocExtensionsOptions(services))
            .UseServiceProviderFactory(new ServiceProviderFactory(configureContainer));
}
