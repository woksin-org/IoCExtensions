// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Woksin.Extensions.Configurations.Internal;

public static class ConfigurationAdder
{
    public static void AddConfigurationDefinitionToServices(IServiceCollection services, Type configurationType, string configurationPath, ConfigurationOptions configurationOptions)
    {
        var definitionType = typeof(ConfigurationObjectDefinition<>).MakeGenericType(configurationType);
        var definition = Activator.CreateInstance(definitionType, configurationPath, configurationOptions)!;
        services.AddSingleton(definitionType, definition);
        services.AddSingleton(typeof(IAmAConfigurationObjectDefinition), provider => provider.GetRequiredService(definitionType));
        AddChangeTokenSource(services, configurationType);
        AddValidation(services, configurationType, configurationOptions);
    }

    static void AddValidation(IServiceCollection services, Type configurationType, ConfigurationOptions configurationOptions)
    {
        var optionsBuilderType = typeof(OptionsBuilder<>).MakeGenericType(configurationType);
        var optionsBuilder = Activator.CreateInstance(optionsBuilderType, services, null);
        if (configurationOptions.ValidateOnStartup)
        {
            var validateOnStartMethod = typeof(OptionsBuilderExtensions).GetMethod(nameof(OptionsBuilderExtensions.ValidateOnStart), BindingFlags.Static | BindingFlags.Public)!; 
            var genericMethod = validateOnStartMethod.MakeGenericMethod(configurationType);
            genericMethod.Invoke(null, [optionsBuilder]);
        }
        if (configurationOptions.ValidateDataAnnotations)
        {
            var validateDataAnnotationsMethod = typeof(OptionsBuilderDataAnnotationsExtensions).GetMethod(
                nameof(OptionsBuilderDataAnnotationsExtensions.ValidateDataAnnotations),
                BindingFlags.Static | BindingFlags.Public)!; 
            var genericMethod = validateDataAnnotationsMethod.MakeGenericMethod(configurationType);
            genericMethod.Invoke(null, [optionsBuilder]);
        }
        
    }

    // Need to add ConfigurationChangeTokenSource manually to listen to changes in the configuration
    static void AddChangeTokenSource(IServiceCollection services, Type configurationType)
    {
        var serviceType = typeof(IOptionsChangeTokenSource<>).MakeGenericType(configurationType);
        var implementationType = typeof(ConfigurationChangeTokenSource<>).MakeGenericType(configurationType);
        services.TryAddSingleton(serviceType, provider => ActivatorUtilities.CreateInstance(provider, implementationType));
    }
}
