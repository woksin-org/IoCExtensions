// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Woksin.Extensions.Tenancy.Strategies;

namespace Woksin.Extensions.Tenancy.AspNetCore.Strategies;

/// <summary>
/// Represents an <see cref="ITenantResolutionStrategy"/> that resolves the tenant identifier by looking at the provided headers in order.
/// </summary>
public partial class HeaderStrategy : ITenantResolutionStrategy
{
    /// <summary>
    /// The default fallback header to look for the tenant identifier in the headers.
    /// </summary>
    public const string DefaultTenantIdHeader = "Tenant-Id";

    readonly string[] _headers;

    /// <summary>
    /// The default implementation of <see cref="HeaderStrategy"/>.
    /// </summary>
    public static readonly HeaderStrategy Default = WithHeaders(DefaultTenantIdHeader);

    /// <summary>
    /// Creates a <see cref="HeaderStrategy"/> with only the provided headers.
    /// </summary>
    /// <param name="headers">The headers to look for the tenant identifier.</param>
    /// <returns>The created <see cref="HeaderStrategy"/>.</returns>
    public static HeaderStrategy WithHeaders(params string[] headers) => new(headers);

    /// <summary>
    /// Creates a <see cref="HeaderStrategy"/> with the provided headers and falls back to the <see cref="DefaultTenantIdHeader"/>.
    /// </summary>
    /// <param name="headers">The headers to first look for the tenant identifier.</param>
    /// <returns>The created <see cref="HeaderStrategy"/>.</returns>
    public static HeaderStrategy DefaultWithHeaders(params string[] headers) => new([.. headers, DefaultTenantIdHeader]);

    /// <summary>
    /// Initializes a new instance of the <see cref="HeaderStrategy"/> class.
    /// </summary>
    /// <param name="headers">The headers to look for the </param>
    protected HeaderStrategy(string[] headers)
    {
        _headers = headers;
    }

    /// <inheritdoc />
    public Task<string?> Resolve(object resolutionContext)
    {
        if (resolutionContext is not HttpContext context)
        {
            throw new ArgumentException("Expected resolution context to be HttpContext", nameof(resolutionContext));
        }

        var logger = context.RequestServices.GetService<ILogger<HeaderStrategy>>() ?? NullLogger<HeaderStrategy>.Instance;
        foreach (var header in _headers)
        {
            LogTryingToGetTenantId(logger, header);
            if (!context.Request.Headers.TryGetValue(header, out var tenantIdString) || tenantIdString.Count == 0)
            {
                continue;
            }
            string tenantId;
            if (tenantIdString.Count == 1)
            {
                tenantId = tenantIdString.ToString();
                LogGotTenantId(logger, tenantId, header);
                return Task.FromResult<string?>(tenantId);
            }
            tenantId = tenantIdString[0]!;
            LogMultipleValuesForHeader(logger, tenantId, header);
            return Task.FromResult<string?>(tenantId);
        }
        return Task.FromResult<string?>(null);
    }

    [LoggerMessage(0, LogLevel.Debug, "Trying to get tenant id from header '{Header}'")]
    static partial void LogTryingToGetTenantId(ILogger logger, string header);

    [LoggerMessage(1, LogLevel.Debug, "Got tenant id '{TenantId}' from header '{Header}'")]
    static partial void LogGotTenantId(ILogger logger, string tenantId, string header);

    [LoggerMessage(2, LogLevel.Warning, "Multiple values for header '{Header}'. Using the first tenant id '{TenantId}'")]
    static partial void LogMultipleValuesForHeader(ILogger logger, string tenantId, string header);
}
