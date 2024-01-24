// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.DependencyInjection;
using Woksin.Extensions.Configurations.Internal;

namespace Woksin.Extensions.Configurations;

public class ConfigurationBuilder : BaseConfigurationBuilder<ConfigurationBuilder>
{
    internal ConfigurationBuilder(IServiceCollection services, string[] configurationPrefixes)
        : base(services, configurationPrefixes)
    {
    }

    protected override ConfigurationBuilder Builder => this;
}
