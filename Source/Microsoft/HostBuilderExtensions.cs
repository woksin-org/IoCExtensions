using System.Reflection;
using IoCExtensions.Provider;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IoCExtensions.Microsoft;

/// <summary>
/// Extension methods for <see cref="IHostBuilder"/>.
/// </summary>
public static class HostBuilderExtensions
{
	/// <summary>
	/// Use the Microsoft IoCExtensions implementation.
	/// </summary>
	/// <param name="builder">The <see cref="IHostBuilder"/>.</param>
	/// <param name="entryAssemblyName">The entry point assembly name to discover services in.</param>
	/// <param name="configureOptions">The callback for configuring <see cref="IoCExtensionsOptions"/>.</param>
	/// <param name="configureContainer">The callback for configuring <see cref="IServiceCollection"/>.</param>
	/// <returns>The <see cref="IHostBuilder"/> for continuation.</returns>
	public static IHostBuilder UseMicrosoftIoCExtensions(
	    this IHostBuilder builder,
		string entryAssemblyName,
	    Action<IoCExtensionsOptions>? configureOptions = default,
	    Action<IServiceCollection>? configureContainer = default) =>
        UseMicrosoftIoCExtensions(builder, _ => IoCExtensionsOptionsConfigurator.Configure(_,entryAssemblyName, configureOptions), configureContainer);

	/// <summary>
    /// Use the Microsoft IoCExtensions implementation.
    /// </summary>
    /// <param name="builder">The <see cref="IHostBuilder"/> to modify.</param>
    /// <param name="entryAssembly">The entry point assembly to discover services in.</param>
    /// <param name="configureOptions">The callback for configuring <see cref="IoCExtensionsOptions"/>.</param>
    /// <param name="configureContainer">The callback for configuring <see cref="IServiceCollection"/>.</param>
    /// <returns>The <see cref="IHostBuilder"/> for continuation.</returns>
    public static IHostBuilder UseMicrosoftIoCExtensions(
		this IHostBuilder builder,
		Assembly entryAssembly,
		Action<IoCExtensionsOptions>? configureOptions = default,
		Action<IServiceCollection>? configureContainer = default) =>
        UseMicrosoftIoCExtensions(builder, _ => IoCExtensionsOptionsConfigurator.Configure(_, entryAssembly, configureOptions), configureContainer);
    
    /// <summary>
    /// Use the Microsoft IoCExtensions implementation.
    /// </summary>
    /// <param name="builder">The <see cref="IHostBuilder"/> to modify.</param>
    /// <param name="configureOptions">The callback for configuring <see cref="IoCExtensionsOptions"/>.</param>
    /// <param name="configureContainer">The callback for configuring <see cref="IServiceCollection"/>.</param>
    /// <returns>The <see cref="IHostBuilder"/> for continuation.</returns>
    public static IHostBuilder UseMicrosoftIoCExtensions(
        this IHostBuilder builder,
        Action<IoCExtensionsOptions>? configureOptions = default,
        Action<IServiceCollection>? configureContainer = default) =>
        UseMicrosoftIoCExtensions(builder, _ => IoCExtensionsOptionsConfigurator.Configure(_, configureOptions), configureContainer);
    
    static IHostBuilder UseMicrosoftIoCExtensions(
        IHostBuilder builder,
        Action<IServiceCollection> addIocExtensionsOptions,
        Action<IServiceCollection>? configureContainer) =>
        builder
            .ConfigureServices((_, services) => addIocExtensionsOptions(services))
            .UseServiceProviderFactory(new ServiceProviderFactory(configureContainer));
}
