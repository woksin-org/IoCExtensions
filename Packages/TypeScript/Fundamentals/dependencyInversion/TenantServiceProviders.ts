// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Container, interfaces } from 'inversify';

import { applyDynamicResolver } from './Internal/Extensions/applyDynamicResolver';
import { applyToContainerAndCreatedChildren } from './Internal/Extensions/applyToContainerAndCreatedChildren';
import { DelegatingResolver } from './Internal/Extensions/DelegatingResolver';
import { InversifyServiceBinder } from './Internal/Implementations/InversifyServiceBinder';
import { InversifyServiceProvider } from './Internal/Implementations/InversifyServiceProvider';
import { MetadataReader } from './Internal/MetadataReader';
import { DefaultServiceProvider } from './DefaultServiceProvider';
import { IServiceProvider } from './IServiceProvider';
import { IServiceProviderBuilder } from './IServiceProviderBuilder';
import { ITenantServiceProviders } from './ITenantServiceProviders';
import { TenantServiceProviderNotConfigured } from './TenantServiceProviderNotConfigured';
import { TenantId } from './TenantId';

/**
 * Represents an implementation of {@link ITenantServiceProviders}.
 */
export class TenantServiceProviders extends ITenantServiceProviders {
    private readonly _rootContainer: interfaces.Container;
    private readonly _tenantContainers: Map<string, IServiceProvider>;

    /**
     * Initializes a new instance of the {@link TenantServiceProviders} class.
     * @param {IServiceProvider} baseServiceProvider - The base service provider to use.
     * @param {IServiceProviderBuilder} bindings - The bindings to bind in the service provider.
     * @param {string[]} tenantIds - The tenants to create service providers for.
     */
    constructor(
        baseServiceProvider: IServiceProvider,
        bindings: IServiceProviderBuilder,
        tenantIds: readonly string[],
    ) {
        super();

        if (baseServiceProvider instanceof DefaultServiceProvider) {
            this._rootContainer = baseServiceProvider.container;
        } else if (baseServiceProvider instanceof InversifyServiceProvider) {
            this._rootContainer = baseServiceProvider.container.createChild();
        } else {
            const container = new Container();
            applyDynamicResolver(container, new DelegatingResolver(baseServiceProvider));
            this._rootContainer = container;
        }

        const metadataReader = new MetadataReader();
        applyToContainerAndCreatedChildren(this._rootContainer, (container) => {
            container.applyCustomMetadataReader(metadataReader);
        });

        const binder = new InversifyServiceBinder(this._rootContainer);
        bindings.bindAllServices(binder);

        this._tenantContainers = new Map<string, IServiceProvider>();
        for (const tenantId of tenantIds) {
            const tenantContainer = this._rootContainer.createChild();
            tenantContainer.bind(TenantId).toConstantValue(tenantId);

            const tenantBinder = new InversifyServiceBinder(tenantContainer);
            bindings.bindAllTenantServices(tenantBinder, tenantId);

            this._tenantContainers.set(tenantId.toString(), new InversifyServiceProvider(tenantContainer));
        }
    }

    /** @inheritdoc */
    forTenant(tenantId: string): IServiceProvider {
        if (!this._tenantContainers.has(tenantId)) {
            throw new TenantServiceProviderNotConfigured(tenantId);
        }

        return this._tenantContainers.get(tenantId)!;
    }
}
