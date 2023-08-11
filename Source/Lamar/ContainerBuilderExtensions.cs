// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Lamar;
using System.Reflection;
using IoCExtensions.Registry.Types;
using Microsoft.Extensions.DependencyInjection;

namespace IoCExtensions.Lamar;

/// <summary>
/// Extension methods for <see cref="ServiceRegistry"/>.
/// </summary>
static class ContainerBuilderExtensions
{
    /// <summary>
    /// Registers a set of discovered <see cref="ClassesByLifeTime"/> in the <see cref="ServiceRegistry"/>.
    /// </summary>
    /// <param name="builder">The container builder to register types in.</param>
    /// <param name="classes">The classes grouped by lifecycle to register.</param>
    public static void RegisterClassesByLifecycle(this ServiceRegistry builder, ClassesByLifeTime classes)
    {
        foreach (var implementation in classes.SingletonClasses)
        {
            foreach (var interfaceType in GetImplementedInterfaces(implementation))
            {
                RegisterService(builder, implementation, interfaceType, ServiceLifetime.Singleton);
            }
        }
        foreach (var implementation in classes.ScopedClasses)
        {
            foreach (var interfaceType in GetImplementedInterfaces(implementation))
            {
                RegisterService(builder, implementation, interfaceType, ServiceLifetime.Scoped);
            }
        }
        foreach (var implementation in classes.TransientClasses)
        {
            foreach (var interfaceType in GetImplementedInterfaces(implementation))
            {
                RegisterService(builder, implementation, interfaceType, ServiceLifetime.Transient);
            }
        }
    }

    /// <summary>
    /// Registers a set of discovered <see cref="ClassesByLifeTime"/> in the <see cref="ServiceRegistry"/>.
    /// </summary>
    /// <param name="builder">The container builder to register types in.</param>
    /// <param name="classes">The classes grouped by lifecycle to register.</param>
    public static void RegisterClassesByLifecycleAsSelf(this ServiceRegistry builder, ClassesByLifeTime classes)
    {
        foreach (var serviceType in classes.SingletonClasses)
        {
            RegisterService(builder, serviceType, serviceType, ServiceLifetime.Singleton);
        }
        foreach (var serviceType in classes.ScopedClasses)
        {
            RegisterService(builder, serviceType, serviceType, ServiceLifetime.Scoped);
        }
        foreach (var serviceType in classes.TransientClasses)
        {
            RegisterService(builder, serviceType, serviceType, ServiceLifetime.Transient);
        }
    }

    static void RegisterService(ServiceRegistry builder, Type implementation, Type service, ServiceLifetime lifetime)
    {
        service = MaybeGetServiceAsOpenGeneric(service, implementation);
        var constructorInstance = builder.For(service).Use(implementation);
        switch (lifetime)
        {
            case ServiceLifetime.Singleton:
                constructorInstance.Singleton();
                break;
            case ServiceLifetime.Scoped:
                constructorInstance.Scoped();
                break;
            case ServiceLifetime.Transient:
                constructorInstance.Transient();
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
