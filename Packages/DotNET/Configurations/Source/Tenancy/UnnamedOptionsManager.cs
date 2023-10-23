// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.Options;

namespace Woksin.Extensions.Configurations.Tenancy;

/// <summary>
/// Copied from .NET Runtime
/// </summary>
/// <typeparam name="TOptions">The options type.</typeparam>
sealed class UnnamedOptionsManager<TOptions> :
    IOptions<TOptions>
    where TOptions : class
{
    readonly IOptionsFactory<TOptions> _factory;
    volatile object? _syncObj;
    volatile TOptions? _value;

    public UnnamedOptionsManager(IOptionsFactory<TOptions> factory) => _factory = factory;

    public TOptions Value
    {
        get
        {
            if (_value is not null)
            {
                return _value;
            }

            lock (_syncObj ?? Interlocked.CompareExchange(ref _syncObj, new object(), null) ?? _syncObj)
            {
                // ReSharper disable once NonAtomicCompoundOperator
                return _value ??= _factory.Create(Options.DefaultName);
            }
        }
    }
}
