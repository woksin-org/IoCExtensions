// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.DependencyInjection;

namespace IoCExtensions;

/// <summary>
/// Defines a system that can add services to a <typeparamref name="TContainerBuilder"/>.
/// </summary>
/// <typeparam name="TContainerBuilder">The type of the container builder.</typeparam>
public interface ICanAddServices<in TContainerBuilder>
	where TContainerBuilder : notnull
{
    /// <summary>
    /// Adds services to the <typeparamref name="TContainerBuilder"/>.
    /// </summary>
    /// <param name="builder">The container builder to add services into.</param>
    void AddTo(TContainerBuilder builder);
}
/// <summary>
/// Defines a system that can add services to a <see cref="IServiceCollection"/>.
/// </summary>
public interface ICanAddServices : ICanAddServices<IServiceCollection>
{
}
