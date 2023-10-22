// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Woksin.Extensions.IoC;
using Woksin.Extensions.IoC.Tenancy;

namespace Woksin.Extensions.Configurations.Tenancy;

/// <summary>
/// Represents an implementation of <see cref="ConfigurationsExtensionOptionsFactory{TOptions}"/> that will throw an exception when attempting to resolve options for a tenant configuration.
/// </summary>
/// <typeparam name="TOptions">The configuration type.</typeparam>
[DisableAutoRegistration]
public class RootContainerConfigurationsExtensionOptionsFactory<TOptions> : ConfigurationsExtensionOptionsFactory<TOptions>
    where TOptions : class
{
    public RootContainerConfigurationsExtensionOptionsFactory(
        IConfiguration configuration,
        ConfigurationPrefix configurationPrefix,
        IEnumerable<ConfigurationObjectDefinition<TOptions>> definitions,
        IEnumerable<IConfigureOptions<TOptions>> setups,
        IEnumerable<IPostConfigureOptions<TOptions>> postConfigures,
        IEnumerable<IValidateOptions<TOptions>> validations)
        : base(configuration, configurationPrefix, definitions, setups, postConfigures, validations)
    {
        if (typeof(TOptions).IsPerTenant())
        {
            throw new CannotResolveTenantConfigurationFromRootContainer(typeof(TOptions));
        }
    }
}
