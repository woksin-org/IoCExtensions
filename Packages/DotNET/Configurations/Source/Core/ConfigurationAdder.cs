// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Woksin.Extensions.Configurations.Internal;

public static class ConfigurationAdder
{
    public static void AddToServices(IServiceCollection services, Type configurationType, string configurationPath, BinderOptions binderOptions )
    {
        var definitionType = typeof(ConfigurationObjectDefinition<>).MakeGenericType(configurationType);
        var definition = Activator.CreateInstance(definitionType, configurationPath, binderOptions)!;
        services.AddSingleton(definitionType, definition);
        services.AddSingleton(typeof(IAmAConfigurationObjectDefinition), provider => provider.GetRequiredService(definitionType));
        AddChangeTokenSource(services, configurationType);
    }

    // Need to add ConfigurationChangeTokenSource manually to listen to changes in the configuration
    static void AddChangeTokenSource(IServiceCollection services, Type configurationType)
    {
        var serviceType = typeof(IOptionsChangeTokenSource<>).MakeGenericType(configurationType);
        var implementationType = typeof(ConfigurationChangeTokenSource<>).MakeGenericType(configurationType);
        services.AddSingleton(serviceType, provider => ActivatorUtilities.CreateInstance(provider, implementationType));
    }
}
