// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Autofac;
using Lamar;
using Microsoft.Extensions.DependencyInjection;

namespace IoCExtensions.given;

public class AutofacServiceAdder : IoCExtensions.Autofac.ICanAddServices
{
	public void AddTo(ContainerBuilder services)
	{
		services.RegisterType<ExplicitlyAddedTransientService>().AsSelf();
	}
}

public class LamarServiceAdder : IoCExtensions.Lamar.ICanAddServices
{
    public void AddTo(ServiceRegistry services)
    {
        services.Use<ExplicitlyAddedTransientService>();
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
