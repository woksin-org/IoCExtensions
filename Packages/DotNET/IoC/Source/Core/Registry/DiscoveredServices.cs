// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reflection;
using IoCExtensions.Lifetime;
using IoCExtensions.Registry.Attributes;
using IoCExtensions.Registry.Types;
using Microsoft.Extensions.DependencyInjection;

namespace IoCExtensions.Registry;

/// <summary>
/// Represents the discovered services that should be configured in the IoC container.
/// </summary>
public sealed class DiscoveredServices<TContainerBuilder>
	where TContainerBuilder : notnull
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DiscoveredServices{TContainerBuilder}"/> class.
    /// </summary>
    /// <param name="options">The options.</param>
    /// <param name="builder">The container builder.</param>
	internal DiscoveredServices(IoCExtensionsOptions options, TContainerBuilder builder)
	{
		AdditionalServices = new ServiceCollection();
		
		// ReSharper disable PossibleMultipleEnumeration
		var discoveredClasses = TypeScanner.GetAllExportedTypesInRuntimeAssemblies(options, out var assemblies);
		var groupedClassesToRegisterAsSelf =
			ClassesByLifeTime.Create(
				options.DefaultLifetime,
				discoveredClasses.Where(_ => Attribute.IsDefined(_, typeof(RegisterAsSelfAttribute))));
		discoveredClasses = IgnoreTypes(options, discoveredClasses);
		discoveredClasses = FilterServiceAdders(discoveredClasses, builder);
		discoveredClasses = discoveredClasses.IgnoreClassesWithAttribute<DisableAutoRegistrationAttribute>();
		var groupedClasses = ClassesByLifeTime.Create(
			options.DefaultLifetime,
			discoveredClasses.Where(_ => !options.DisableRegistrationByConvention || Attribute.IsDefined(_, typeof(WithLifetimeAttribute))));

		ClassesToRegister = groupedClasses;
		ClassesToRegisterAsSelf = groupedClassesToRegisterAsSelf;
		Assemblies = assemblies.ToArray();
	}
	
	/// <summary>
	/// Gets the additional <see cref="IServiceCollection"/> to register. 
	/// </summary>
	public IServiceCollection AdditionalServices { get; }
	
	/// <summary>
	/// Gets the <see cref="ClassesByLifeTime"/> to be registered as their implementing types.
	/// </summary>
	public ClassesByLifeTime ClassesToRegister { get; }

	/// <summary>
	/// Gets the <see cref="ClassesByLifeTime"/> to be registered as themselves.
	/// </summary>
    public ClassesByLifeTime ClassesToRegisterAsSelf { get; }

	/// <summary>
	/// The assemblies that were searched through.
	/// </summary>
	public IReadOnlyCollection<Assembly> Assemblies { get; }

	static IEnumerable<Type> IgnoreTypes(IoCExtensionsOptions options, IEnumerable<Type> discoveredTypes) => options
		.IgnoredBaseTypes
		.Aggregate(discoveredTypes, (current, ignoredType) => current.FilterClassesImplementing(ignoredType, _ => true, out _));
	
	static IEnumerable<Type> FilterServiceAddersForContainerBuilder<T>(IEnumerable<Type> discoveredClasses, T builder)
		where T : notnull
	{
		discoveredClasses = discoveredClasses
			.FilterClassesImplementing(typeof(ICanAddServices<T>), _ => true, out var classesThatCanAddServices)
			.FilterClassesImplementing(
				typeof(ICanAddServicesForTypesWith<,>),
				_ =>
				{
					var genericParameters = _.GetGenericArguments();
					return genericParameters.Length switch
					{
						2 => genericParameters[1] == typeof(T),
						_ => false
					};
				},
				out var classesThatCanAddServicesForAttribute);
		var instancesThatCanAddServices = CreateInstanceOfWithDefaultConstructor<ICanAddServices<T>>(classesThatCanAddServices);
		var instancesThatCanAddServicesForAttribute = classesThatCanAddServicesForAttribute.Select(_ => ServicesForTypesWith.CreateBuilderFor<T>(_, discoveredClasses));
		var serviceAdders = instancesThatCanAddServices.Concat(instancesThatCanAddServicesForAttribute).ToList();
		foreach (var adder in serviceAdders)
		{
			adder.AddTo(builder);
		}

		return discoveredClasses;
	}

	IEnumerable<Type> FilterServiceAdders(IEnumerable<Type> discoveredClasses, TContainerBuilder builder)
	{
		discoveredClasses = FilterServiceAddersForContainerBuilder(discoveredClasses, builder);
		return typeof(TContainerBuilder) != typeof(IServiceCollection)
			? FilterServiceAddersForContainerBuilder(discoveredClasses, AdditionalServices)
			: discoveredClasses;
	}

	static IEnumerable<T> CreateInstanceOfWithDefaultConstructor<T>(IEnumerable<Type> classes)
		where T : class
		=> classes.Select(type =>
		{
			try
			{
				if (Activator.CreateInstance(type) is not T instance)
				{
					throw new CouldNotCreateInstanceOfType(type);
				}

				return instance;
			}
			catch (Exception exception) when (exception is not CouldNotCreateInstanceOfType)
			{
				throw new CouldNotCreateInstanceOfType(type, exception);
			}
		});
}
