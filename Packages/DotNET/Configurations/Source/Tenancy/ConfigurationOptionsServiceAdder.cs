// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Woksin.Extensions.Configurations.Internal;
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
        => AddConfigurationObjectDefinition(services, type, attribute, type.IsPerTenantDecoratedClass());

    static void AddConfigurationObjectDefinition(IServiceCollection services, Type type, ConfigurationAttribute attribute, bool perTenant)
    {
        if (perTenant)
        {
            services.AddTenantScopedServices((tenantId, tenantServices) => ConfigurationAdder.AddToServices(
                tenantServices,
                type,
                ConfigurationPath.Combine(tenantId, attribute.ConfigurationPath),
                attribute.BinderOptions));
        }
        else
        {
            ConfigurationAdder.AddToServices(services, type, attribute.ConfigurationPath, attribute.BinderOptions);
        }
    }
}
