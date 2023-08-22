// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { IServiceProvider } from './IServiceProvider';

/**
 * Defines a system that can provide and instance of {@link IServiceProvider} with services for a specific tenant.
 */
export abstract class ITenantServiceProviders {
    /**
     * Gets the service provider for the specified tenant.
     * @param {string} tenantId - The tenant to get a service provider for.
     * @returns {IServiceProvider} The service provider for the tenant.
     */
    abstract forTenant(tenantId: string): IServiceProvider;
}
