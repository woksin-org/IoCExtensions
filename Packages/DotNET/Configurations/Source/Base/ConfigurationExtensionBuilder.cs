// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.DependencyInjection;
using Woksin.Extensions.Configurations.Internal;

namespace Woksin.Extensions.Configurations;

public class ConfigurationExtensionBuilder : BaseConfigurationExtensionBuilder<ConfigurationExtensionBuilder>
{
    internal ConfigurationExtensionBuilder(IServiceCollection services, string[] configurationPrefixes)
        : base(services, configurationPrefixes)
    {
    }

    protected override ConfigurationExtensionBuilder Builder => this;

    protected override void AddAdditionalServicesFor(Type optionsType)
    {
        // No need to add anything more
    }
}
