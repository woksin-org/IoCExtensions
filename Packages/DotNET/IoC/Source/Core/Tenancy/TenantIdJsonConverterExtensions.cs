// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;

namespace Woksin.Extensions.IoC.Tenancy;

/// <summary>
/// Extension methods for adding <see cref="TenantIdJsonConverter"/> to <see cref="JsonSerializerOptions"/>.
/// </summary>
public static class TenantIdJsonConverterExtensions
{
    /// <summary>
    /// Adds a <see cref="TenantIdJsonConverter"/> to the <see cref="JsonSerializerOptions"/>.
    /// </summary>
    /// <param name="builder">The <see cref="IMvcBuilder"/>.</param>
    /// <returns>The <see cref="IMvcBuilder"/>.</returns>
    public static IMvcBuilder AddTenantIdJsonConverter(this IMvcBuilder builder)
    {
        builder.AddJsonOptions(options => options.JsonSerializerOptions.AddTenantIdJsonConverter());
        return builder;
    }

    /// <summary>
    /// Adds a <see cref="TenantIdJsonConverter"/> to the <see cref="JsonSerializerOptions"/>.
    /// </summary>
    /// <param name="options">The <see cref="JsonSerializerOptions"/>.</param>
    /// <returns>The <see cref="JsonSerializerOptions"/>.</returns>
    public static JsonSerializerOptions AddTenantIdJsonConverter(this JsonSerializerOptions options)
    {
        options.Converters.Add(new TenantIdJsonConverter());
        return options;
    }

    /// <summary>
    /// Adds a <see cref="TenantIdJsonConverter"/> to the <see cref="JsonSerializerOptions"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    /// <returns>The <see cref="services"/>.</returns>
    /// <remarks>
    /// This method adds the <see cref="TenantIdJsonConverter"/> to both <see cref="Microsoft.AspNetCore.Mvc.JsonOptions"/> and <see cref="Microsoft.AspNetCore.Http.Json.JsonOptions"/>
    /// so that it is used when serializing/deserializing <see cref="TenantId"/> in both MVC and API controllers.
    /// </remarks>
    public static IServiceCollection AddTenantIdJsonConverter(this IServiceCollection services)
        => services
            .Configure<Microsoft.AspNetCore.Mvc.JsonOptions>(options => options.JsonSerializerOptions.AddTenantIdJsonConverter())
            .Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options => options.SerializerOptions.AddTenantIdJsonConverter());
}
