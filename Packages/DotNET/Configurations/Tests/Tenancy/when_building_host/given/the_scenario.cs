// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Woksin.Extensions.Configurations.Tenancy.given;
using Woksin.Extensions.IoC.Tenancy;
using Woksin.Extensions.Specifications.XUnit;

namespace Woksin.Extensions.Configurations.Tenancy.when_building_host.given;

public partial class the_scenario : Specification
{
	protected IHost? host;
	protected Assembly entry_assembly;

    protected TenantId the_tenant;

	protected IOptions<ConfigWithOnePrefix> config_with_one_prefix;
	protected IOptions<TenantConfigWithOnePrefix> tenant_config_with_one_prefix;
	protected IOptions<ConfigWithTwoPrefixes> config_with_two_prefixes;
	protected IOptions<TenantConfigWithTwoPrefixes> tenant_config_with_two_prefixes;
	protected IOptions<ConfigWithComplexObject> config_with_complex_object;
	protected IOptions<TenantConfigWithComplexObject> tenant_config_with_complex_object;
	protected IOptions<ConfigWithNestedConfiguration> config_with_nested_configuration;
	protected IOptions<TenantConfigWithNestedConfiguration> tenant_config_with_nested_configuration;
	protected IOptions<NestedConfig> nested_config;
	protected IOptions<TenantNestedConfig> tenant_nested_config;

	protected ConfigWithOnePrefix expected_config_with_one_prefix;
	protected TenantConfigWithOnePrefix expected_tenant_config_with_one_prefix;
	protected ConfigWithTwoPrefixes expected_config_with_two_prefixes;
	protected TenantConfigWithTwoPrefixes expected_tenant_config_with_two_prefixes;
	protected ConfigWithComplexObject expected_config_with_complex_object;
	protected TenantConfigWithComplexObject expected_tenant_config_with_complex_object;
	protected ConfigWithNestedConfiguration expected_config_with_nested_configuration;
	protected TenantConfigWithNestedConfiguration expected_tenant_config_with_nested_configuration;
	protected NestedConfig expected_nested_config;
	protected TenantNestedConfig expected_tenant_nested_config;

	void Establish()
    {
        the_tenant = "the_tenant";
		entry_assembly = typeof(a_host_builder).Assembly;
	}

	void Destroy()
	{
		host?.Dispose();
	}

	protected void SetupAllServices()
	{
		SetService(ref config_with_one_prefix);
		SetTenantService(ref tenant_config_with_one_prefix);
		SetService(ref config_with_two_prefixes);
		SetTenantService(ref tenant_config_with_two_prefixes);
		SetService(ref config_with_complex_object);
		SetTenantService(ref tenant_config_with_complex_object);
		SetService(ref config_with_nested_configuration);
		SetTenantService(ref tenant_config_with_nested_configuration);
		SetService(ref nested_config);
		SetTenantService(ref tenant_nested_config);

		expected_config_with_one_prefix = new ConfigWithOnePrefix { SomeInt = 43 };
        expected_tenant_config_with_one_prefix = new TenantConfigWithOnePrefix { SomeInt = 44 };
		expected_config_with_two_prefixes = new ConfigWithTwoPrefixes { SomeInt = 44 };
		expected_tenant_config_with_two_prefixes = new TenantConfigWithTwoPrefixes { SomeInt = 45 };
		expected_config_with_complex_object = new ConfigWithComplexObject
			{ SomeInt = 45, SomeComplex = new SomeComplex { AStringValue = "value" } };
        expected_tenant_config_with_complex_object = new TenantConfigWithComplexObject
			{ SomeInt = 46, SomeComplex = new SomeComplex { AStringValue = "value" } };
		expected_config_with_nested_configuration = new ConfigWithNestedConfiguration
			{ SomeInt = 46, SomeNestedConfig = new NestedConfig{SomeNestedInt = 47} };
        expected_tenant_config_with_nested_configuration = new TenantConfigWithNestedConfiguration
			{ SomeInt = 47, SomeNestedConfig = new TenantNestedConfig{SomeNestedInt = 48} };
		expected_nested_config = new NestedConfig { SomeNestedInt = 47 };
		expected_tenant_nested_config = new TenantNestedConfig { SomeNestedInt = 48 };
	}

	// ReSharper disable once RedundantAssignment
	void SetService<T>(ref T services) => services = host!.Services.GetService<T>()!;
	void SetTenantService<T>(ref T services)
    {
        var provider = host!.Services.GetRequiredService<ITenantScopedServiceProviders>().ForTenant(the_tenant); 
        services = provider.GetService<T>()!;
    }
}
