// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.


import { Constructor } from '@woksin/extensions.types';

/**
 * The exception that gets thrown when the client is configured with to use an unknown service provider.
 */
export class UnknownServiceProviderType extends Error {
    /**
     * Initializes a new instance of the {@link UnknownServiceProviderType} class.
     * @param {Constructor<any>} type - The type of the configured service provider.
     */
    constructor(type: Constructor<any>) {
        super(`The service provider type ${type.name} is not supported.`);
    }
}
