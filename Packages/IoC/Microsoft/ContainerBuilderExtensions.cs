// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reflection;
using IoCExtensions.Registry.Types;
using Microsoft.Extensions.DependencyInjection;

namespace IoCExtensions.Microsoft;

/// <summary>
/// Extension methods for <see cref="IServiceCollection"/>.
/// </summary>
static class ContainerBuilderExtensions
{
    /// <summary>
    /// Registers a set of discovered <see cref="ClassesByLifeTime"/> in the <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="builder">The container builder to register types in.</param>
    /// <param name="classes">The classes grouped by lifecycle to register.</param>
    public static void RegisterClassesByLifecycle(this IServiceCollection builder, ClassesByLifeTime classes)
    {
        builder.RegisterTypes(classes.SingletonClasses, ServiceLifetime.Singleton);
        builder.RegisterTypes(classes.ScopedClasses, ServiceLifetime.Scoped);
        builder.RegisterTypes(classes.TransientClasses, ServiceLifetime.Transient);
    }

    /// <summary>
    /// Registers a set of discovered <see cref="ClassesByLifeTime"/> in the <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="builder">The container builder to register types in.</param>
    /// <param name="classes">The classes grouped by lifecycle to register.</param>
    public static void RegisterClassesByLifecycleAsSelf(this IServiceCollection builder, ClassesByLifeTime classes)
    {
        builder.RegisterTypesAsSelf(classes.SingletonClasses, ServiceLifetime.Singleton);
        builder.RegisterTypesAsSelf(classes.ScopedClasses, ServiceLifetime.Scoped);
        builder.RegisterTypesAsSelf(classes.TransientClasses, ServiceLifetime.Transient);
    }

    static void RegisterTypes(this IServiceCollection builder, IEnumerable<Type> services, ServiceLifetime lifetime)
    {
	    foreach (var implementationType in services)
	    {
		    foreach (var implementedInterface in GetImplementedInterfaces(implementationType))
		    {
			    builder.AddWithLifeTime(implementedInterface, implementationType, lifetime);
		    }
	    }
    }
    static void RegisterTypesAsSelf(this IServiceCollection builder, IEnumerable<Type> services, ServiceLifetime lifetime)
    {
	    foreach (var service in services)
	    {
		    builder.AddWithLifeTime(service, service, lifetime);
	    }
    }

    static void AddWithLifeTime(this IServiceCollection builder, Type service, Type implementation,
	    ServiceLifetime lifetime)
    {
	    service = MaybeGetServiceAsOpenGeneric(service, implementation);
	    switch (lifetime)
	    {
		    case ServiceLifetime.Singleton:
			    builder.AddSingleton(service, implementation);
			    break;
		    case ServiceLifetime.Scoped:
			    builder.AddScoped(service, implementation);
			    break;
		    case ServiceLifetime.Transient:
			    builder.AddTransient(service, implementation);
			    break;
		    default:
			    throw new ArgumentOutOfRangeException(nameof(lifetime), lifetime, $"Lifetime {lifetime} not supported");
	    }
    }
    static Type MaybeGetServiceAsOpenGeneric(Type service, Type implementation)
		=> IsOpenGeneric(implementation) && service.IsGenericType
			? GetOpenGenericType(service)
			: service;

    static IEnumerable<Type> GetImplementedInterfaces(Type type)
    {
	    var interfaces = type.GetInterfaces().Where(i => i != typeof(IDisposable));
	    return type.IsInterface ? interfaces.Append(type).ToArray() : interfaces.ToArray();
    }

    static bool IsOpenGeneric(Type type) => type.GetTypeInfo().IsGenericTypeDefinition;

    static Type GetOpenGenericType(Type type)
    {
	    var openGenericDefinition = type.GetTypeInfo().GetGenericTypeDefinition();
	    return openGenericDefinition;
    }
}
