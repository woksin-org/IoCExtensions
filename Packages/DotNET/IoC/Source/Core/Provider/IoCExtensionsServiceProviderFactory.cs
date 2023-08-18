// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using IoCExtensions.Registry;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace IoCExtensions.Provider;

/// <summary>
/// Represents a base implementation of <see cref="IServiceProviderFactory{TContainerBuilder}"/> for IoCExtensions service provider
/// implementations.
/// </summary>
/// <typeparam name="TContainerBuilder">The <see cref="IServiceProviderFactory{TContainerBuilder}"/> of the container builder.</typeparam>
[DisableAutoRegistration]
public abstract class IoCExtensionsServiceProviderFactory<TContainerBuilder> : IServiceProviderFactory<TContainerBuilder>
	where TContainerBuilder : notnull
{
	IServiceCollection? _services;
	
	/// <inheritdoc />
	public TContainerBuilder CreateBuilder(IServiceCollection services)
	{
		_services = services;
		return CreateContainerBuilder(services);
	}
	
	/// <inheritdoc />
	public IServiceProvider CreateServiceProvider(TContainerBuilder containerBuilder) =>
		CreateServiceProvider(
			containerBuilder,
			new DiscoveredServices<TContainerBuilder>(CreateOptions(), containerBuilder));

	/// <summary>
	/// Create the <typeparamref name="TContainerBuilder"/> from the <see cref="IServiceCollection"/>.
	/// </summary>
	/// <param name="services">The service collection.</param>
	/// <returns>The <typeparamref name="TContainerBuilder"/>.</returns>
	protected abstract TContainerBuilder CreateContainerBuilder(IServiceCollection services);
	
	/// <summary>
	/// Create the <see cref="IServiceProvider"/> using the <typeparamref name="TContainerBuilder"/>.
	/// </summary>
	/// <param name="containerBuilder">The container builder.</param>
	/// <param name="discoveredServices">The discovered services.</param>
	/// <returns>The <see cref="IServiceProvider"/>.</returns>
	protected abstract IServiceProvider CreateServiceProvider(TContainerBuilder containerBuilder,
		DiscoveredServices<TContainerBuilder> discoveredServices);

	IoCExtensionsOptions CreateOptions()
	{
		var optionsConfigurators = _services!
			.Where(_ => _.ImplementationInstance is IConfigureOptions<IoCExtensionsOptions>)
			.Select(_ => _.ImplementationInstance as IConfigureOptions<IoCExtensionsOptions>);
		var options = new IoCExtensionsOptions();
		foreach (var configurator in optionsConfigurators)
		{
			configurator?.Configure(options);
		}

		return options;
	}
}
