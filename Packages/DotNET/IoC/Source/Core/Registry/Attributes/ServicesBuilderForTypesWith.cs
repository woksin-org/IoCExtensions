// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Woksin.Extensions.IoC.Registry.Attributes;

/// <summary>
/// Represents an implementation of <see cref="ICanAddServices{T}"/>.
/// </summary>
/// <typeparam name="TAttribute">The <see cref="Type"/> of the attribute.</typeparam>
/// <typeparam name="TContainerBuilder">The type of the container builder.</typeparam>
[DisableAutoRegistration]
class ServicesBuilderForTypesWith<TAttribute, TContainerBuilder> : ICanAddServices<TContainerBuilder>
    where TAttribute : Attribute
	where TContainerBuilder : notnull
{
	readonly ICanAddServicesForTypesWith<TAttribute, TContainerBuilder> _builder;
	readonly Dictionary<Type, IEnumerable<TAttribute>> _typesWithAttribute = new();

    public ServicesBuilderForTypesWith(ICanAddServicesForTypesWith<TAttribute, TContainerBuilder> builder, IEnumerable<Type> discoveredClasses)
    {
        _builder = builder;
        foreach (var type in discoveredClasses)
        {
            var attributes = Attribute.GetCustomAttributes(type, typeof(TAttribute)).Cast<TAttribute>().ToArray();
            if (attributes.Any())
            {
                _typesWithAttribute.Add(type, attributes);
            }
        }
    }

    /// <inheritdoc />
    public void AddTo(TContainerBuilder builder)
    {
        foreach (var (type, attributes) in _typesWithAttribute)
        {
            foreach (var attribute in attributes)
            {
                _builder.AddServiceFor(type, attribute, builder);
            }
        }
    }
}
