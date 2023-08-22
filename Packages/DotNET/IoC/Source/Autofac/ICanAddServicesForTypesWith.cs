// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Autofac;

namespace Woksin.Extensions.IoC.Autofac;

/// <inheritdoc />
public interface ICanAddServicesForTypesWith<in TAttribute> : ICanAddServicesForTypesWith<TAttribute, ContainerBuilder>
	where TAttribute : Attribute
{
}