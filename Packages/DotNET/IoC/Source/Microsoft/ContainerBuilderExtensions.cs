// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Woksin.Extensions.IoC.Registry.Types;
using Woksin.Extensions.IoC.Tenancy;

namespace Woksin.Extensions.IoC.Microsoft;

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
        builder.RegisterTypesPerTenant(classes.PerTenantSingletonClasses, ServiceLifetime.Singleton);
        builder.RegisterTypes(classes.ScopedClasses, ServiceLifetime.Scoped);
        builder.RegisterTypesPerTenant(classes.PerTenantScopedClasses, ServiceLifetime.Scoped);
        builder.RegisterTypes(classes.TransientClasses, ServiceLifetime.Transient);
        builder.RegisterTypesPerTenant(classes.PerTenantTransientClasses, ServiceLifetime.Transient);
    }

    /// <summary>
    /// Registers a set of discovered <see cref="ClassesByLifeTime"/> in the <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="builder">The container builder to register types in.</param>
    /// <param name="classes">The classes grouped by lifecycle to register.</param>
    public static void RegisterClassesByLifecycleAsSelf(this IServiceCollection builder, ClassesByLifeTime classes)
    {
        builder.RegisterTypesAsSelf(classes.SingletonClasses, ServiceLifetime.Singleton);
        builder.RegisterTypesAsSelfPerTenant(classes.PerTenantSingletonClasses, ServiceLifetime.Singleton);
        builder.RegisterTypesAsSelf(classes.ScopedClasses, ServiceLifetime.Scoped);
        builder.RegisterTypesAsSelfPerTenant(classes.PerTenantScopedClasses, ServiceLifetime.Scoped);
        builder.RegisterTypesAsSelf(classes.TransientClasses, ServiceLifetime.Transient);
        builder.RegisterTypesAsSelfPerTenant(classes.PerTenantTransientClasses, ServiceLifetime.Transient);
    }

    static void RegisterTypes(this IServiceCollection builder, IEnumerable<Type> services, ServiceLifetime lifetime)
    {
	    foreach (var implementationType in services)
	    {
		    foreach (var implementedInterface in GetImplementedInterfaces(implementationType))
		    {
			    AddWithLifeTime(implementedInterface, implementationType, lifetime)(builder);
		    }
	    }
    }
    static void RegisterTypesPerTenant(this IServiceCollection builder, IEnumerable<Type> services, ServiceLifetime lifetime)
    {
	    foreach (var implementationType in services)
	    {
		    foreach (var implementedInterface in GetImplementedInterfaces(implementationType))
		    {
			    builder.AddTenantScopedServices(AddWithLifeTime(implementedInterface, implementationType, lifetime));
		    }
	    }
    }
    static void RegisterTypesAsSelf(this IServiceCollection builder, IEnumerable<Type> services, ServiceLifetime lifetime)
    {
	    foreach (var service in services)
	    {
		    AddWithLifeTime(service, service, lifetime)(builder);
	    }
    }
    static void RegisterTypesAsSelfPerTenant(this IServiceCollection builder, IEnumerable<Type> services, ServiceLifetime lifetime)
    {
	    foreach (var service in services)
	    {
		    builder.AddTenantScopedServices(AddWithLifeTime(service, service, lifetime));
	    }
    }

    static Action<IServiceCollection> AddWithLifeTime(Type service, Type implementation, ServiceLifetime lifetime)
    {
	    service = MaybeGetServiceAsOpenGeneric(service, implementation);
        return lifetime switch
        {
            ServiceLifetime.Singleton => builder => builder.AddSingleton(service, implementation),
            ServiceLifetime.Scoped => builder => builder.AddScoped(service, implementation),
            ServiceLifetime.Transient => builder => builder.AddTransient(service, implementation),
            _ => throw new ArgumentOutOfRangeException(nameof(lifetime), lifetime, $"Lifetime {lifetime} not supported")
        };
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

    static Type GetOpenGenericType(Type type) => type.GetTypeInfo().GetGenericTypeDefinition();
}
