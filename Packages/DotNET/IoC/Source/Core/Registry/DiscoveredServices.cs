// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Woksin.Extensions.IoC.Registry.Attributes;
using Woksin.Extensions.IoC.Registry.Types;
using Woksin.Extensions.IoC.Tenancy;

namespace Woksin.Extensions.IoC.Registry;

/// <summary>
/// Represents the discovered services that should be configured in the IoC container.
/// </summary>
/// <typeparam name="TContainerBuilder">The <see cref="Type"/> of the container builder.</typeparam>
public sealed class DiscoveredServices<TContainerBuilder>
	where TContainerBuilder : notnull
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DiscoveredServices{TContainerBuilder}"/> class.
    /// </summary>
    /// <param name="settings">The options.</param>
    /// <param name="builder">The container builder.</param>
	internal DiscoveredServices(IoCSettings settings, TContainerBuilder builder)
	{
		AdditionalServices = new ServiceCollection();
        AdditionalServices.AddDefaultMultiTenancyServices();
		// ReSharper disable PossibleMultipleEnumeration
		var discoveredClasses = TypeScanner.GetAllExportedTypesInRuntimeAssemblies(settings, out var assemblies);
		var groupedClassesToRegisterAsSelf =
			ClassesByLifeTime.Create(
				settings.DefaultLifetime,
				discoveredClasses.Where(type => Attribute.IsDefined(type, typeof(RegisterAsSelfAttribute))));
		discoveredClasses = IgnoreTypes(settings, discoveredClasses);
		discoveredClasses = FilterServiceAdders(discoveredClasses, builder);
		discoveredClasses = discoveredClasses.IgnoreClassesWithAttribute<DisableAutoRegistrationAttribute>();
		ClassesToRegister = ClassesByLifeTime.Create(
			settings.DefaultLifetime,
			discoveredClasses.Where(type => settings.EnableRegistrationByConvention || Attribute.IsDefined(type, typeof(WithLifetimeAttribute))));
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

	static IEnumerable<Type> IgnoreTypes(IoCSettings settings, IEnumerable<Type> discoveredTypes) => settings
		.IgnoredBaseTypes
		.Aggregate(discoveredTypes, (current, ignoredType) => current.FilterClassesImplementing(ignoredType, _ => true, out _));

	static IEnumerable<Type> FilterServiceAddersForContainerBuilder<T>(IEnumerable<Type> discoveredClasses, T builder)
		where T : notnull
	{
		discoveredClasses = discoveredClasses
			.FilterClassesImplementing(typeof(ICanAddServices<T>), _ => true, out var classesThatCanAddServices)
			.FilterClassesImplementing(
				typeof(ICanAddServicesForTypesWith<,>),
				type =>
				{
					var genericParameters = type.GetGenericArguments();
					return genericParameters.Length switch
					{
						2 => genericParameters[1] == typeof(T),
						_ => false
					};
				},
				out var classesThatCanAddServicesForAttribute);
		var instancesThatCanAddServices = CreateInstanceOfWithDefaultConstructor<ICanAddServices<T>>(classesThatCanAddServices);
		var instancesThatCanAddServicesForAttribute = classesThatCanAddServicesForAttribute.Select(_ => ServicesForTypesWith.CreateBuilderFor<T>(_, discoveredClasses));
		foreach (var adder in instancesThatCanAddServices.Concat(instancesThatCanAddServicesForAttribute))
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
