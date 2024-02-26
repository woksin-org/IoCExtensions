// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Woksin.Extensions.Tenancy.AspNetCore.Strategies;
using Woksin.Extensions.Tenancy.Strategies;

namespace Woksin.Extensions.Tenancy.AspNetCore;

public static class TenancyBuilderExtensions
{
    /// <summary>
    /// Adds the default <see cref="ITenantResolutionStrategy"/>.
    /// </summary>
    /// <param name="builder">The <see cref="TenancyBuilder{TTenant}"/>.</param>
    /// <typeparam name="TTenant">The <see cref="Type"/> of the <see cref="ITenantInfo"/>.</typeparam>
    /// <returns>The builder for continuation.</returns>
    public static TenancyBuilder<TTenant> WithDefaultStrategies<TTenant>(this TenancyBuilder<TTenant> builder)
        where TTenant : class, ITenantInfo, new()
        => builder.WithStrategy(HeaderStrategy.Default);
}
