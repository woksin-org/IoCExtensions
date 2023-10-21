// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Woksin.Extensions.IoC;

/// <summary>
/// Represents the settings used when creating the service provider.
/// </summary>
public class IoCSettings
{
    /// <summary>
    /// Gets or sets the string representing the first part of the name of the assemblies that will be included in the service type search.
    /// The type scanner will search for services in assemblies <see cref="Assembly"/> starting with the given name.
    /// </summary>
    /// <remarks>
    /// If your application has the prefix assembly name Some.Assembly and you have class projects with assembly name
    /// Some.Assembly.LibraryX then <see cref="AssemblySearchNamePrefix"/> should be Some.Assembly so that every related
    /// assembly is used in the service discovery.
    /// </remarks>
    public string AssemblySearchNamePrefix { get; set; } = null!;

	/// <summary>
	/// Gets the list of additional assemblies to include in the search.
	/// </summary>
    public IList<Assembly> AdditionalAssemblies { get; } = new List<Assembly>();

	/// <summary>
	/// Gets the assembly names to ignore. 
	/// </summary>
	public IList<string> IgnoredAssemblyNames { get; } = new List<string>();

	/// <summary>
	/// Gets the assembly names to ignore. 
	/// </summary>
	public IList<Assembly> IgnoredAssemblies { get; } = new List<Assembly>();

	/// <summary>
	/// Gets the types to ignore. Every class implementing one of these types will be ignored.
	/// </summary>
	public IList<Type> IgnoredBaseTypes { get; } = new List<Type>();

	/// <summary>
	/// Gets or sets the default fallback <see cref="ServiceLifetime"/> for services that will be automatically registered.
	/// </summary>
	public ServiceLifetime DefaultLifetime { get; set; } = ServiceLifetime.Scoped;

    /// <summary>
    /// Gets or sets the value indicating whether to enable automatic registration of services by convention.
    /// If set to false only services with <see cref="WithLifetimeAttribute"/>, <see cref="RegisterAsSelfAttribute" /> and services added through
    /// <see cref="ICanAddServices{TContainerBuilder}" /> and <see cref="ICanAddServicesForTypesWith{TAttribute, TContainerBuilder}" /> will be registered in the service provider.
    /// </summary>
    public bool EnableRegistrationByConvention { get; set; }
}
