namespace IoCExtensions.Configuration.when_building_host.using_a_config_prefix.given;

public class a_host_builder : when_building_host.given.a_host_builder
{
	void Establish()
	{
		host_builder
			.UseIoCExtensionsConfigurations("Some", "Prefix");
	}
}
