using Microsoft.Extensions.Configuration;

namespace IoCExtensions.Configuration.when_building_host.using_a_config_prefix.using_appsettings.given;

public class a_host_builder : using_a_config_prefix.given.a_host_builder
{
	void Establish()
	{
		host_builder.ConfigureAppConfiguration((x, y) => y.AddJsonFile("appsettings_with_prefix.json"));
	}
}
