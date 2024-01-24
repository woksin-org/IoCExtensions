// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Builder;

namespace Woksin.Extensions.Tenancy.AspNetCore;

public static class HostExtensions
{
    /// <summary>
    /// Sets the <see cref="TenancyMiddleware"/> in the request pipeline.
    /// </summary>
    /// <param name="builder">The <see cref="IApplicationBuilder"/> builder.</param>
    /// <returns>The builder for continuation.</returns>
    public static IApplicationBuilder UseTenancy(this IApplicationBuilder builder)
        => builder.UseMiddleware<TenancyMiddleware>();
}
