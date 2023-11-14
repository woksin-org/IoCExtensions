// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Woksin.Extensions.Configurations;

/// <summary>
/// Extension methods for <see cref="IServiceProvider"/>.
/// </summary>
public static class ServiceProviderExtensions
{
    /// <summary>
    /// Gets a snapshot of the configuration object.
    /// </summary>
    /// <param name="serviceProvider">The <see cref="IServiceProvider"/>.</param>
    /// <typeparam name="TOptions">The options class.</typeparam>
    /// <returns>The snapshot of the options class.</returns>
    public static TOptions GetConfigurationSnapshot<TOptions>(this IServiceProvider serviceProvider)
        where TOptions : class
        => serviceProvider.GetRequiredService<IOptionsSnapshot<TOptions>>().Value;

    /// <summary>
    /// Gets a monitor of the configuration object.
    /// </summary>
    /// <param name="serviceProvider">The <see cref="IServiceProvider"/>.</param>
    /// <typeparam name="TOptions">The options class.</typeparam>
    /// <returns>The <see cref="IOptionsMonitor{TOptions}"/> of the options class.</returns>
    public static IOptionsMonitor<TOptions> GetConfigurationMonitor<TOptions>(this IServiceProvider serviceProvider)
        where TOptions : class
        => serviceProvider.GetRequiredService<IOptionsMonitor<TOptions>>();
}
