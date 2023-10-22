// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Woksin.Extensions.IoC;
using Woksin.Extensions.IoC.Tenancy;

namespace Woksin.Extensions.Configurations.Tenancy;

/// <summary>
/// Represents an implementation of <see cref="ICanAddServicesForTypesWith{TAttribute,TContainerBuilder}"/> for <see cref="ConfigurationAttribute"/>.
/// </summary>
[DisableAutoRegistration]
public class ConfigurationOptionsServiceAdder : ICanAddServicesForTypesWith<ConfigurationAttribute>
{
    /// <inheritdoc />
    public void AddServiceFor(Type type, ConfigurationAttribute attribute, IServiceCollection services)
        => AddConfigurationObjectDefinition(services, type, attribute.ConfigurationPath, type.IsPerTenant());

    static void AddConfigurationObjectDefinition(IServiceCollection services, Type type, string configurationPath, bool perTenant)
    {
        if (perTenant)
        {
            services.AddTenantScopedServices((tenantId, tenantServices) => AddConfigurationObjectDefinitionTypes(
                tenantServices,
                typeof(ConfigurationObjectDefinition<>).MakeGenericType(type),
                ConfigurationPath.Combine(tenantId, configurationPath)));
        }
        else
        {
            AddConfigurationObjectDefinitionTypes(services, typeof(ConfigurationObjectDefinition<>).MakeGenericType(type), configurationPath);
        }
    }

    static void AddConfigurationObjectDefinitionTypes(IServiceCollection services, Type definitionType, string configurationPath)
    {
        var definition = Activator.CreateInstance(definitionType, configurationPath)!;
        services.AddSingleton(definitionType, definition);
        services.AddSingleton(typeof(IAmAConfigurationObjectDefinition), provider => provider.GetRequiredService(definitionType));
    }
}
