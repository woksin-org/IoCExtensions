// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Woksin.Extensions.IoC.Provider;

/// <summary>
/// Represents methods for adding <see cref="IoCSettings"/> the the <see cref="IServiceCollection"/>. 
/// </summary>
public static class IoCOptionsConfigurator
{
    /// <summary>
    /// Adds the <see cref="IoCSettings"/> configuration to the <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="entryAssembly">The entry point assembly name.</param>
    /// <param name="configureOptions">The optional callback for configuring the <see cref="IoCSettings"/>.</param>
    /// <returns>The builder for continuation.</returns>
	public static IServiceCollection Configure(IServiceCollection services, string entryAssembly, Action<IoCSettings>? configureOptions = default)
	{
		services
			.AddOptions<IoCSettings>()
			.Configure(_ =>
			{
				_.AssemblySearchNamePrefix = entryAssembly;
				configureOptions?.Invoke(_);
			});
		return services;
	}
    
    /// <summary>
    /// Adds the <see cref="IoCSettings"/> configuration to the <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="entryAssembly">The entry point assembly.</param>
    /// <param name="configureOptions">The optional callback for configuring the <see cref="IoCSettings"/>.</param>
    /// <returns>The builder for continuation.</returns>
	public static IServiceCollection Configure(IServiceCollection services, Assembly entryAssembly, Action<IoCSettings>? configureOptions = default)
	{
		services
			.AddOptions<IoCSettings>()
			.Configure(_ =>
			{
				_.AssemblySearchNamePrefix = entryAssembly.GetName().Name!;
				configureOptions?.Invoke(_);
			});
		return services;
	}

    
    /// <summary>
    /// Adds the <see cref="IoCSettings"/> configuration to the <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configureOptions">The optional callback for configuring the <see cref="IoCSettings"/>.</param>
    /// <returns>The builder for continuation.</returns>
    public static IServiceCollection Configure(IServiceCollection services, Action<IoCSettings>? configureOptions = default) =>
        Configure(services, Assembly.GetExecutingAssembly(), configureOptions);
}
