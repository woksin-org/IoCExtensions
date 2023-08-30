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
    /// <param name="classes">The classes grouped by lifecycle to register.</param>
    public static void RegisterClassesByLifecycle(this ContainerBuilder builder, ClassesByLifeTime classes)
    {
        builder.RegisterTypes(classes.SingletonClasses.ToArray()).AsImplementedInterfaces().SingleInstance();
        builder.RegisterOpenGenericTypes(classes.SingletonClasses, ServiceLifetime.Singleton, false);
        builder.RegisterTypes(classes.ScopedClasses.ToArray()).AsImplementedInterfaces().InstancePerLifetimeScope();
        builder.RegisterOpenGenericTypes(classes.ScopedClasses, ServiceLifetime.Scoped, false);
        builder.RegisterTypes(classes.TransientClasses.ToArray()).AsImplementedInterfaces();
        builder.RegisterOpenGenericTypes(classes.TransientClasses, ServiceLifetime.Transient, false);
    }

    /// <summary>
    /// Registers a set of discovered <see cref="ClassesByLifeTime"/> in the <see cref="ContainerBuilder"/>.
    /// </summary>
    /// <param name="builder">The container builder to register types in.</param>
    /// <param name="classes">The classes grouped by lifecycle to register.</param>
    public static void RegisterClassesByLifecycleAsSelf(this ContainerBuilder builder, ClassesByLifeTime classes)
    {
        builder.RegisterTypes(classes.SingletonClasses.ToArray()).AsSelf().SingleInstance();
        builder.RegisterOpenGenericTypes(classes.SingletonClasses, ServiceLifetime.Singleton, true);
        builder.RegisterTypes(classes.ScopedClasses.ToArray()).AsSelf().InstancePerLifetimeScope();
        builder.RegisterOpenGenericTypes(classes.ScopedClasses, ServiceLifetime.Scoped, true);
        builder.RegisterTypes(classes.TransientClasses.ToArray()).AsSelf();
        builder.RegisterOpenGenericTypes(classes.TransientClasses, ServiceLifetime.Transient, true);
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

