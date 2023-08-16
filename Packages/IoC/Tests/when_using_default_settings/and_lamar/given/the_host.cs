using IoCExtensions.given;
using IoCExtensions.Lamar;

namespace IoCExtensions.when_using_default_settings.and_lamar.given;

public class the_host : a_host_builder
{
	void Establish()
	{
		host_builder.UseLamarIoCExtensions(entry_assembly);
		BuildHost();
	}
}
