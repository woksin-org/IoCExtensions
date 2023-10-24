// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.DependencyInjection;

namespace Woksin.Extensions.IoC;

/// <summary>
/// Indicates the <see cref="ServiceLifetime" />
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public sealed class WithLifetimeAttribute : Attribute
{
	/// <summary>
	/// Initializes a new instance of the <see cref="WithLifetimeAttribute"/>-
	/// </summary>
	/// <param name="lifetime">THe <see cref="ServiceLifetime"/> to register the service as.</param>
    public WithLifetimeAttribute(ServiceLifetime lifetime)
    {
        Lifetime = lifetime;
    }

    /// <summary>
    /// Gets the <see cref="ServiceLifetime" />.
    /// </summary>
    public ServiceLifetime Lifetime { get; }
}
