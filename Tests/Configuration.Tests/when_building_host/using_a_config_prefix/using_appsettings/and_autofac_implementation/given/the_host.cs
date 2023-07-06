using IoCExtensions.Autofac;

namespace IoCExtensions.Configuration.when_building_host.using_a_config_prefix.using_appsettings.and_autofac_implementation.given;

public class the_host : using_appsettings.given.a_host_builder
{
	void Establish()
	{
		host_builder
			.UseAutofacIoCExtensions(entry_assembly);
		BuildHost();
	}
}
