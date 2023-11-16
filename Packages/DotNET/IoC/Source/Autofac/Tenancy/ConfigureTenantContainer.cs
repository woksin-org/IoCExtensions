// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Autofac;
using Woksin.Extensions.IoC.Tenancy;

namespace Woksin.Extensions.IoC.Autofac.Tenancy;

/// <summary>
/// The callback for configuring the Tenant-specific IoC containers.
/// </summary>
/// <param name="tenant">The <see cref="TenantId"/>.</param>
/// <param name="containerBuilder">The <see cref="ContainerBuilder" /> to configure.</param>
public delegate void ConfigureTenantContainer(TenantId tenant, ContainerBuilder containerBuilder);
