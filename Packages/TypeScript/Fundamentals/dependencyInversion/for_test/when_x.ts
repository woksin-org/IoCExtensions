// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { describeThis } from '@woksin/typescript.testing';
import { Container, inject } from 'inversify';
import { createRootServiceProvider } from '../createRootServiceProvider';
import { TenantId } from '../TenantId';

describeThis(__filename, () => {
    class X {
        get x() {return 2;}
    }

    const container = new Container();
    container.bind(X).toSelf();
    class Y {
        constructor(@inject(X) readonly x: X) {}
    }
    container.bind(Y).toSelf();
    const services = createRootServiceProvider(container);

    const tenant = services.get(TenantId);
    const y = services.get(Y);

    it(('should create y'), () => y.x.x.should.equal(2));
});
