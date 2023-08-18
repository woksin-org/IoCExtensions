// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.ObjectModel;
using IoCExtensions.Lifetime;
using Microsoft.Extensions.DependencyInjection;

namespace IoCExtensions.Registry.Types;

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
        var classesByLifecycle = classes.ToLookup(_ => _.GetLifetimeWithFallback(fallbackLifetime));
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
		SingletonClasses = new ReadOnlyCollection<Type>(singletonClasses.ToList());
		ScopedClasses = new ReadOnlyCollection<Type>(scopedClasses.ToList()); ;
		TransientClasses = new ReadOnlyCollection<Type>(transientClasses.ToList());;
	}

	/// <summary>
    /// Gets the discovered classes that should be registered as singleton.
    /// </summary>
    public IReadOnlyCollection<Type> SingletonClasses { get; }

    /// <summary>
    /// Gets the discovered classes that should be registered as scoped.
    /// </summary>
    public IReadOnlyCollection<Type> ScopedClasses { get; }

    /// <summary>
    /// Gets the discovered classes that should be registered as transient.
    /// </summary>
    public IReadOnlyCollection<Type> TransientClasses { get; }
}
