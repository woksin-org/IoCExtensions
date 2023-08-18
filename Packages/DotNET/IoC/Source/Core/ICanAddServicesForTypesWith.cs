// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.DependencyInjection;

namespace Woksin.Extensions.IoC;

/// <summary>
/// Defines a system that can add services to a <typeparamref name="TContainerBuilder"/> for classes with a specific <see cref="Attribute"/>.
/// </summary>
/// <typeparam name="TAttribute">The type of the <see cref="Attribute"/>.</typeparam>
/// <typeparam name="TContainerBuilder">The type of the container builder.</typeparam>
public interface ICanAddServicesForTypesWith<in TAttribute, in TContainerBuilder>
    where TAttribute : Attribute
	where TContainerBuilder : notnull
{
    /// <summary>
    /// Adds services for the <see cref="Type"/> with the <typeparamref name="TAttribute"/> attribute to the <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="type">The type with the attribute.</param>
    /// <param name="attribute">The attribute instance.</param>
    /// <param name="services">The builder to add services into.</param>
    void AddServiceFor(Type type, TAttribute attribute, TContainerBuilder services);
}
/// <summary>
/// Defines a system that can add services to a <see cref="IServiceCollection"/> for classes with a specific <see cref="Attribute"/>.
/// </summary>
/// <typeparam name="TAttribute">The type of the <see cref="Attribute"/>.</typeparam>
public interface ICanAddServicesForTypesWith<in TAttribute> : ICanAddServicesForTypesWith<TAttribute, IServiceCollection>
	where TAttribute : Attribute
{
}
