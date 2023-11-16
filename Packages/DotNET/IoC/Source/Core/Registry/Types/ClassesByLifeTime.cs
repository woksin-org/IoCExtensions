// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.ObjectModel;
using Microsoft.Extensions.DependencyInjection;
using Woksin.Extensions.IoC.Lifetime;
using Woksin.Extensions.IoC.Tenancy;

namespace Woksin.Extensions.IoC.Registry.Types;

/// <summary>
/// Represents a set of discovered classes grouped by DI lifecycle.
/// </summary>
public class ClassesByLifeTime
{
    /// <summary>
    /// Groups classes by <see cref="ServiceLifetime"/>.
    /// </summary>
    /// <param name="fallbackLifetime">The fallback <see cref="ServiceLifetime"/>.</param>
    /// <param name="classes">The <see cref="IEnumerable{T}"/> of <see cref="Type"/> classes to group.</param>
    /// <returns>The <see cref="ClassesByLifeTime"/>.</returns>
    public static ClassesByLifeTime Create(ServiceLifetime fallbackLifetime, IEnumerable<Type> classes)
    {
        var classesByLifecycle = classes.ToLookup(type => type.GetLifetimeWithFallback(fallbackLifetime));
        return new ClassesByLifeTime(
            classesByLifecycle[ServiceLifetime.Singleton].ToArray(),
            classesByLifecycle[ServiceLifetime.Scoped].ToArray(),
            classesByLifecycle[ServiceLifetime.Transient].ToArray());
    }
	/// <summary>
	/// Initializes a new instance of the <see cref="ClassesByLifeTime"/> class.
	/// </summary>
	/// <param name="singletonClasses">The implementations to be registered as singleton.</param>
	/// <param name="scopedClasses">The implementations to be registered as scoped.</param>
	/// <param name="transientClasses">The implementations to be registered as transient.</param>
	public ClassesByLifeTime(
		IEnumerable<Type> singletonClasses,
		IEnumerable<Type> scopedClasses,
		IEnumerable<Type> transientClasses)
	{
		SingletonClasses = new ReadOnlyCollection<Type>(singletonClasses
            .FilterClassesWithAttribute<PerTenantAttribute>(out var perTenantSingleton).ToList());
		PerTenantSingletonClasses = new ReadOnlyCollection<Type>(perTenantSingleton.ToList());
		ScopedClasses = new ReadOnlyCollection<Type>(scopedClasses
            .FilterClassesWithAttribute<PerTenantAttribute>(out var perTenantScoped).ToList());
		PerTenantScopedClasses = new ReadOnlyCollection<Type>(perTenantScoped.ToList());
		TransientClasses = new ReadOnlyCollection<Type>(transientClasses
            .FilterClassesWithAttribute<PerTenantAttribute>(out var perTenantTransient).ToList());
		PerTenantTransientClasses = new ReadOnlyCollection<Type>(perTenantTransient.ToList());
	}

	/// <summary>
    /// Gets the discovered classes that should be registered as singleton.
    /// </summary>
    public IReadOnlyCollection<Type> SingletonClasses { get; }

    /// <summary>
    /// Gets the discovered classes that should be registered as per tenant singleton.
    /// </summary>
    public IReadOnlyCollection<Type> PerTenantSingletonClasses { get; }

    /// <summary>
    /// Gets the discovered classes that should be registered as scoped.
    /// </summary>
    public IReadOnlyCollection<Type> ScopedClasses { get; }

    /// <summary>
    /// Gets the discovered classes that should be registered as per tenant scoped.
    /// </summary>
    public IReadOnlyCollection<Type> PerTenantScopedClasses { get; }

    /// <summary>
    /// Gets the discovered classes that should be registered as transient.
    /// </summary>
    public IReadOnlyCollection<Type> TransientClasses { get; }

    /// <summary>
    /// Gets the discovered classes that should be registered as per tenant transient.
    /// </summary>
    public IReadOnlyCollection<Type> PerTenantTransientClasses { get; }
}
