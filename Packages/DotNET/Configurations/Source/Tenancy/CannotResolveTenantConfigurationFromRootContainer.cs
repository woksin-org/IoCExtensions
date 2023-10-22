// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Woksin.Extensions.Configurations.Tenancy;

/// <summary>
/// Exception that gets thrown when options for a tenant configuration is being resolved from the root container.
/// </summary>
public class CannotResolveTenantConfigurationFromRootContainer : Exception
{
    public CannotResolveTenantConfigurationFromRootContainer(Type tenantConfigurationType)
        : base($"Cannot create tenant configuration '{tenantConfigurationType}' from root container")
    {
    }
}
