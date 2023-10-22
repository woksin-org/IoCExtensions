// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.DependencyInjection;
using Woksin.Extensions.IoC;
using Woksin.Extensions.IoC.Tenancy;

namespace Woksin.Extensions.Configurations.Tenancy;

/// <summary>
/// Represents an implementation of <see cref="ICanAddServicesForTypesWith{TAttribute,TContainerBuilder}"/> for <see cref="ConfigurationAttribute"/>.
/// </summary>
public class ConfigurationOptionsServiceAdder : ICanAddServicesForTypesWith<ConfigurationAttribute>
{
    /// <inheritdoc />
    public void AddServiceFor(Type type, ConfigurationAttribute attribute, IServiceCollection services)
    {
        if (Attribute.IsDefined(type, typeof(PerTenantAttribute)))
        {
            services.AddTenantConfigurationObjectDefinitionFor(type, attribute.ConfigurationPath);
        }
        else
        {
            services.AddConfigurationObjectDefinitionFor(type, attribute.ConfigurationPath);
        }
    }
}
