// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace IoCExtensions.Provider;

/// <summary>
/// Represents methods for adding <see cref="IoCExtensionsOptions"/> the the <see cref="IServiceCollection"/>. 
/// </summary>
public static class IoCExtensionsOptionsConfigurator
{
    /// <summary>
    /// Adds the <see cref="IoCExtensionsOptions"/> configuration to the <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="entryAssembly">The entry point assembly name.</param>
    /// <param name="configureOptions">The optional callback for configuring the <see cref="IoCExtensionsOptions"/>.</param>
    /// <returns>The builder for continuation.</returns>
	public static IServiceCollection Configure(IServiceCollection services, string entryAssembly, Action<IoCExtensionsOptions>? configureOptions = default)
	{
		services
			.AddOptions<IoCExtensionsOptions>()
			.Configure(_ =>
			{
				_.AssemblySearchNamePrefix = entryAssembly;
				configureOptions?.Invoke(_);
			});
		return services;
	}
    
    /// <summary>
    /// Adds the <see cref="IoCExtensionsOptions"/> configuration to the <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="entryAssembly">The entry point assembly.</param>
    /// <param name="configureOptions">The optional callback for configuring the <see cref="IoCExtensionsOptions"/>.</param>
    /// <returns>The builder for continuation.</returns>
	public static IServiceCollection Configure(IServiceCollection services, Assembly entryAssembly, Action<IoCExtensionsOptions>? configureOptions = default)
	{
		services
			.AddOptions<IoCExtensionsOptions>()
			.Configure(_ =>
			{
				_.AssemblySearchNamePrefix = entryAssembly.GetName().Name!;
				configureOptions?.Invoke(_);
			});
		return services;
	}

    
    /// <summary>
    /// Adds the <see cref="IoCExtensionsOptions"/> configuration to the <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configureOptions">The optional callback for configuring the <see cref="IoCExtensionsOptions"/>.</param>
    /// <returns>The builder for continuation.</returns>
    public static IServiceCollection Configure(IServiceCollection services, Action<IoCExtensionsOptions>? configureOptions = default) =>
        Configure(services, Assembly.GetExecutingAssembly(), configureOptions);
}
