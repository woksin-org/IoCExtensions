// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Autofac;
using Microsoft.Extensions.DependencyInjection;

namespace Woksin.Extensions.IoC.Benchmarks;

public class AutofacServiceAdder : Autofac.ICanAddServices
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
