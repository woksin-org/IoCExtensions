// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reflection;
using Autofac;
using Microsoft.Extensions.DependencyInjection;
using Woksin.Extensions.IoC.Registry.Types;

namespace Woksin.Extensions.IoC.Autofac;

/// <summary>
/// Extension methods for <see cref="ContainerBuilder"/>.
/// </summary>
static class ContainerBuilderExtensions
{
    /// <summary>
    /// Registers a set of discovered <see cref="ClassesByLifeTime"/> in the <see cref="ContainerBuilder"/>.
    /// </summary>
    /// <param name="builder">The container builder to register types in.</param>
    /// <param name="singletonClasses">The singleton classes to register.</param>
    /// <param name="scopedClasses">The scoped classes to register.</param>
    /// <param name="transientClasses">The transient classes to register.</param>
    public static void RegisterClassesByLifecycle(
        this ContainerBuilder builder,
        Type[] singletonClasses,
        Type[] scopedClasses,
        Type[] transientClasses)
    {
        builder.RegisterTypes(singletonClasses).AsImplementedInterfaces().SingleInstance();
        builder.RegisterOpenGenericTypes(singletonClasses, ServiceLifetime.Singleton, false);
        builder.RegisterTypes(scopedClasses).AsImplementedInterfaces().InstancePerLifetimeScope();
        builder.RegisterOpenGenericTypes(scopedClasses, ServiceLifetime.Scoped, false);
        builder.RegisterTypes(transientClasses).AsImplementedInterfaces();
        builder.RegisterOpenGenericTypes(transientClasses, ServiceLifetime.Transient, false);
    }

    /// <summary>
    /// Registers a set of discovered <see cref="ClassesByLifeTime"/> in the <see cref="ContainerBuilder"/>.
    /// </summary>
    /// <param name="builder">The container builder to register types in.</param>
    /// <param name="singletonClasses">The singleton classes to register.</param>
    /// <param name="scopedClasses">The scoped classes to register.</param>
    /// <param name="transientClasses">The transient classes to register.</param>
    public static void RegisterClassesByLifecycleAsSelf(this ContainerBuilder builder,
        Type[] singletonClasses,
        Type[] scopedClasses,
        Type[] transientClasses)
    {
        builder.RegisterTypes(singletonClasses).AsSelf().SingleInstance();
        builder.RegisterOpenGenericTypes(singletonClasses, ServiceLifetime.Singleton, true);
        builder.RegisterTypes(scopedClasses).AsSelf().InstancePerLifetimeScope();
        builder.RegisterOpenGenericTypes(scopedClasses, ServiceLifetime.Scoped, true);
        builder.RegisterTypes(transientClasses).AsSelf();
        builder.RegisterOpenGenericTypes(transientClasses, ServiceLifetime.Transient, true);
    }

    static void RegisterOpenGenericTypes(this ContainerBuilder builder, IEnumerable<Type> types, ServiceLifetime lifetime, bool asSelf)
    {
	    foreach (var implementationType in types.Where(IsOpenGeneric))
	    {
		    var registrationBuilder = builder.RegisterGeneric(implementationType);
		    if (asSelf)
		    {
			    registrationBuilder.AsSelf();
		    }
		    else
		    {
			    registrationBuilder.AsImplementedInterfaces();
		    }

		    switch (lifetime)
		    {
			    case ServiceLifetime.Singleton:
				    registrationBuilder.SingleInstance();
				    break;
			    case ServiceLifetime.Scoped:
				    registrationBuilder.InstancePerLifetimeScope();
				    break;
		    }
	    }
    }

    static bool IsOpenGeneric(Type type) => type.GetTypeInfo().IsGenericTypeDefinition;
}

