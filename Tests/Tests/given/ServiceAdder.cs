using Autofac;
using Microsoft.Extensions.DependencyInjection;

namespace IoCExtensions.given;

public class AutofacServiceAdder : IoCExtensions.Autofac.ICanAddServices
{
	public void AddTo(ContainerBuilder services)
	{
		services.RegisterType<ExplicitlyAddedTransientService>().AsSelf();
	}
}

public class ServiceCollectionAddedServiceAdder : ICanAddServices<IServiceCollection>
{
	public void AddTo(IServiceCollection services)
	{
		services.AddTransient<ServiceAddedByServiceCollectionAdder>();
	}
}


public class MicrosoftServiceAdder : ICanAddServices
{
	public void AddTo(IServiceCollection services)
	{
		services.AddTransient<ExplicitlyAddedTransientService>();
	}
}

public class ServiceAddedByServiceCollectionAdder {}

public class ExplicitlyAddedTransientService
{
}
