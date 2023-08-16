using System.Reflection;
using Autofac;
using IoCExtensions.Provider;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IoCExtensions.Autofac;

/// <summary>
/// Extension methods for <see cref="IHostBuilder"/>.
/// </summary>
public static class HostBuilderExtensions
{
	/// <summary>
    /// Use the Autofac IoCExtensions implementation.
	/// </summary>
	/// <param name="builder">The <see cref="IHostBuilder"/> to modify.</param>
	/// <param name="entryAssemblyName">The entry point assembly name to discover services in.</param>
	/// <param name="configureOptions">The callback for configuring <see cref="IoCExtensionsOptions"/>.</param>
	/// <param name="configureContainer">The callback for configuring <see cref="ContainerBuilder"/>.</param>
	/// <returns>The <see cref="IHostBuilder"/> for continuation.</returns>
	public static IHostBuilder UseAutofacIoCExtensions(
		this IHostBuilder builder,
		string entryAssemblyName,
		Action<IoCExtensionsOptions>? configureOptions = default,
		Action<ContainerBuilder>? configureContainer = default) => UseAutofacIoCExtensions(
        builder, _ => IoCExtensionsOptionsConfigurator.Configure(_, entryAssemblyName, configureOptions), configureContainer);

    /// <summary>
    /// Use the Autofac IoCExtensions implementation.
    /// </summary>
    /// <param name="builder">The <see cref="IHostBuilder"/> to modify.</param>
    /// <param name="entryAssembly">The entry point assembly to discover services in.</param>
    /// <param name="configureOptions">The callback for configuring <see cref="IoCExtensionsOptions"/>.</param>
    /// <param name="configureContainer">The callback for configuring <see cref="ContainerBuilder"/>.</param>
    /// <returns>The <see cref="IHostBuilder"/> for continuation.</returns>
    public static IHostBuilder UseAutofacIoCExtensions(
        this IHostBuilder builder,
        Assembly entryAssembly,
        Action<IoCExtensionsOptions>? configureOptions = default,
        Action<ContainerBuilder>? configureContainer = default) => UseAutofacIoCExtensions(
        builder, _ => IoCExtensionsOptionsConfigurator.Configure(_, entryAssembly, configureOptions), configureContainer);
    
    /// <summary>
    /// Use the Autofac IoCExtensions implementation.
    /// </summary>
    /// <param name="builder">The <see cref="IHostBuilder"/> to modify.</param>
    /// <param name="configureOptions">The callback for configuring <see cref="IoCExtensionsOptions"/>.</param>
    /// <param name="configureContainer">The callback for configuring <see cref="ContainerBuilder"/>.</param>
    /// <returns>The <see cref="IHostBuilder"/> for continuation.</returns>
    public static IHostBuilder UseAutofacIoCExtensions(
        this IHostBuilder builder,
        Action<IoCExtensionsOptions>? configureOptions = default,
        Action<ContainerBuilder>? configureContainer = default) => UseAutofacIoCExtensions(
        builder, _ => IoCExtensionsOptionsConfigurator.Configure(_, configureOptions), configureContainer);
    
    
    static IHostBuilder UseAutofacIoCExtensions(
        IHostBuilder builder,
        Action<IServiceCollection> addIocExtensionsOptions,
        Action<ContainerBuilder>? configureContainer) =>
        builder
            .ConfigureServices((_, services) => addIocExtensionsOptions(services))
            .UseServiceProviderFactory(new ServiceProviderFactory(configureContainer));
}
