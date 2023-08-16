using IoCExtensions.Configuration.Parsing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace IoCExtensions.Configuration;

/// <summary>
/// Represents extension methods for adding the IoCExtensions configuration system to a host.
/// </summary>
public static class HostExtensions
{
	/// <summary>
	/// Use the IoCExtensions Configuration system.
	/// </summary>
	/// <param name="builder">The <see cref="IHostBuilder"/>.</param>
	/// <param name="configurationPrefixes">The configuration prefixes.</param>
	/// <returns>The builder for continuation.</returns>
	public static IHostBuilder UseIoCExtensionsConfigurations(this IHostBuilder builder, params string[] configurationPrefixes)
		=> builder.ConfigureServices((_, services) => services.AddIoCExtensionsConfigurations(configurationPrefixes));
	
	/// <summary>
	/// Adds the IoCExtensions Configuration system.
	/// </summary>
	/// <param name="services">The <see cref="IServiceCollection"/>.</param>
	/// <param name="configurationPrefixes">The configuration prefixes.</param>
	/// <returns>The builder for continuation.</returns>
	public static IServiceCollection AddIoCExtensionsConfigurations(this IServiceCollection services, params string[] configurationPrefixes)
	{
		var prefix = ConfigurationPath.Combine(configurationPrefixes);
		if (!string.IsNullOrEmpty(prefix))
		{
			prefix += ConfigurationPath.KeyDelimiter;
		}
		services.AddOptions<IoCExtensionsConfigurationOptions>().Configure(_ => _.Prefix = prefix);
		services.Add(ServiceDescriptor.Singleton(typeof(IOptionsFactory<>), typeof(OptionsFactory<>)));
		services.AddSingleton<IParseConfigurationObjects, ConfigurationParser>();
		services.Configure<IoCExtensionsOptions>(_ => _.AdditionalAssemblies.Add(typeof(HostExtensions).Assembly));
		return services;
	}
}
