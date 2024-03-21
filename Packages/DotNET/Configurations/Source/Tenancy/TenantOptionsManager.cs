// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.Options;

namespace Woksin.Extensions.Configurations.Tenancy;

/// <summary>
/// Implementation of IOptions and IOptionsSnapshot that uses dependency injection for its private cache.
/// </summary>
/// <typeparam name="TOptions">The type of options being configured.</typeparam>
public class TenantOptionsManager<TOptions> : IOptionsSnapshot<TOptions> where TOptions : class
{
    readonly IOptionsFactory<TOptions> _factory;
    readonly IOptionsMonitorCache<TOptions> _cache;

    public TenantOptionsManager(IOptionsFactory<TOptions> factory, IOptionsMonitorCache<TOptions> cache)
    {
        _factory = factory;
        _cache = cache;
    }

    /// <inheritdoc />
    public TOptions Value => Get(Options.DefaultName);

    /// <inheritdoc />
    public TOptions Get(string? name)
    {
        name ??= Options.DefaultName;

        // Store the options in our instance cache.
        return _cache.GetOrAdd(name, () => _factory.Create(name));
    }
}
