// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.DependencyInjection;

namespace IoCExtensions.Configuration;

/// <summary>
/// Represents an implementation of <see cref="ICanAddServicesForTypesWith{TAttribute,TContainerBuilder}"/> for adding <see cref="ConfigurationObjectDefinition{TOptions}"/> to the IoC
/// container for each type with the <see cref="ConfigurationAttribute"/> attribute.
/// </summary>
public sealed class ConfigurationObjects : ICanAddServicesForTypesWith<ConfigurationAttribute>
{
    /// <inheritdoc />
    public void AddServiceFor(Type type, ConfigurationAttribute attribute, IServiceCollection services)
    {
        var definitionType = typeof(ConfigurationObjectDefinition<>).MakeGenericType(type);
        var definition = Activator.CreateInstance(definitionType, attribute)!;
        services.AddSingleton(definitionType, definition);
        services.AddSingleton(typeof(IAmAConfigurationObjectDefinition), _ => _.GetRequiredService(definitionType));
    }
}
