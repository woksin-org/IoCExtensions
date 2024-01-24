// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reflection;

namespace Woksin.Extensions.Tenancy;

/// <summary>
/// Represents extension methods for adding the configuration system to a host.
/// </summary>
public static class HostExtensions
{
	/// <summary>
	/// Use the tenancy system.
	/// </summary>
	/// <param name="builder">The <see cref="IHostBuilder"/>.</param>
    /// <param name="startupAssembly">The startup assembly.</param>
	/// <param name="configurationPrefixes">The configuration prefixes.</param>
	/// <returns>The builder for continuation.</returns>
	public static IHostBuilder UseTenancyExtension(this IHostBuilder builder, Assembly startupAssembly, params string[] configurationPrefixes)
        => builder.ConfigureServices((_, services) => services.AddConfigurationExtension(startupAssembly, configurationPrefixes));

}
