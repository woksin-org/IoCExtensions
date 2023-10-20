// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reflection;
using Woksin.Extensions.Specifications.XUnit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Woksin.Extensions.IoC.Tenancy;

namespace Woksin.Extensions.IoC.given;

public partial class the_scenario : Specification
{
	protected IHost? host;
	protected Assembly entry_assembly;
    protected TenantId the_tenant_id;
	protected Services<ISingletonService> singleton;
	protected Services<ISingletonPerTenantService> singletonPerTenant;
	protected Services<IScopedService> scoped;
	protected Services<IScopedPerTenantService> scopedPerTenant;
	protected Services<ITransientService> transient;
	protected Services<ITransientPerTenantService> transientPerTenant;
	protected Services<IServiceWithoutLifetimeAttribute> service_without_lifetime_attribute;
	protected Services<IPerTenantServiceWithoutLifetimeAttribute> per_tenant_service_without_lifetime_attribute;
	protected Services<ExplicitlyAddedTransientService> explicitly_added_service;
	protected Services<ServiceAddedByServiceCollectionAdder> service_added_by_service_collection_adder;
	protected Services<ClassWithSomeAttribute> service_with_some_attribute;
	protected Services<ClassWithAddedByServiceCollectionAdderAttribute> service_with_some_attribute_added_by_service_callection_adder;
	protected Services<IServiceWithMultipleInterface_1> service_with_multiple_interfaces_1;
	protected Services<IServiceWithMultipleInterface_2<int>> service_with_multiple_interfaces_2;
	protected Services<IServiceWithMultipleInterface_3<int, string>> service_with_multiple_interfaces_3;
	protected Services<ITernaryGenericService<int, string>> ternary_generic_service;
	protected Services<INotAutoRegisteredService> not_auto_registered_service;
	protected Services<ITransitiveBase> transitive_base;
	protected Services<ITransitiveService> transitive_service;
	protected Services<ITransitiveGenericBase<int>> transitive_generic_base;
	protected Services<ITransitiveGenericService<int>> transitive_generic_service;
	protected Services<ITransitiveGenericBase<string>> transitive_open_generic_base;
	protected Services<ITransitiveGenericService<string>> transitive_open_generic_service;
	protected Services<IPartiallyClosedGenericService<int, string>> partially_closed_generic_service;
	protected Services<SelfRegisteredClass> self_registered_class;
	protected Services<PerTenantSelfRegisteredClass> per_tenant_self_registered_class;
	protected Services<SelfRegisteredGenericClass<int>> self_registered_generic_class;
	protected Services<SelfRegisteredClassWithScopedLifetime> self_registered_generic_with_scoped_lifetime;

	protected ServiceLifetime default_service_lifetime;

	List<IDisposable> scopes;


	void Establish()
	{
        the_tenant_id = "some-tenant";
		entry_assembly = typeof(a_host_builder).Assembly;
		default_service_lifetime = new IoCSettings().DefaultLifetime;
		scopes = new List<IDisposable>();
    }

	void Destroy()
	{
		host?.Dispose();
		foreach (var scope in scopes)
		{
			scope.Dispose();
		}
	}

	protected void SetupAllServices()
	{
		SetService(ref singleton);
		SetService(ref singletonPerTenant, true);
		SetService(ref scoped);
		SetService(ref scopedPerTenant, true);
		SetService(ref transient);
		SetService(ref transientPerTenant, true);
		SetService(ref service_without_lifetime_attribute);
		SetService(ref per_tenant_service_without_lifetime_attribute, true);
		SetService(ref explicitly_added_service);
		SetService(ref service_added_by_service_collection_adder);
		SetService(ref service_with_some_attribute);
		SetService(ref service_with_some_attribute_added_by_service_callection_adder);
		SetService(ref service_with_multiple_interfaces_1);
		SetService(ref service_with_multiple_interfaces_2);
		SetService(ref service_with_multiple_interfaces_3);
		SetService(ref ternary_generic_service);
		SetService(ref not_auto_registered_service);
		SetService(ref transitive_base);
		SetService(ref transitive_service);
		SetService(ref transitive_generic_base);
		SetService(ref transitive_generic_service);
		SetService(ref transitive_open_generic_base);
		SetService(ref transitive_open_generic_service);
		SetService(ref partially_closed_generic_service);
		SetService(ref self_registered_class);
		SetService(ref per_tenant_self_registered_class, true);
		SetService(ref self_registered_generic_class);
		SetService(ref self_registered_generic_with_scoped_lifetime);
	}

	// ReSharper disable once RedundantAssignment
	void SetService<T>(ref Services<T> services, bool perTenant = false)
	{
		services = GetServices<T>(perTenant);
	}

	protected Services<TService> GetServices<TService>(bool perTenant)
    {
        var services = perTenant
            ? host.Services.GetService<ITenantScopedServiceProviders>().ForTenant(the_tenant_id)
            : host.Services;
		var scope = services.CreateScope();
		var otherScope = services.CreateScope();
		scopes.Add(scope);
		scopes.Add(otherScope);
		return new Services<TService>(
			services.GetService<TService>(),
			services.GetService<TService>(),
			scope.ServiceProvider.GetService<TService>(),
			otherScope.ServiceProvider.GetService<TService>());
	}

	protected record Services<T>(T? First, T? Second, T? FirstScoped, T? SecondScoped);
}
