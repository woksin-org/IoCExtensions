// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Woksin.Extensions.IoC.Tenancy.Middleware;

/// <summary>
/// Represents a <see cref="TenantIdStrategy"/> that resolves the <see cref="TenantId"/> by looking at the provided headers in order.
/// </summary>
public partial class TenantIdFromHeaderStrategy : TenantIdStrategy
{
    /// <summary>
    /// The default fallback header to look for the <see cref="TenantId"/> in the headers.
    /// </summary>
    public const string DefaultTenantIdHeader = "Tenant-Id";

    readonly string[] _headers;

    /// <summary>
    /// The default implementation of <see cref="TenantIdFromHeaderStrategy"/>.
    /// </summary>
    public static readonly TenantIdFromHeaderStrategy Default = WithHeaders(DefaultTenantIdHeader);

    /// <summary>
    /// Creates a <see cref="TenantIdFromHeaderStrategy"/> with only the provided headers.
    /// </summary>
    /// <param name="headers">The headers to look for the <see cref="TenantId"/>.</param>
    /// <returns>The created <see cref="TenantIdStrategy"/>.</returns>
    public static TenantIdFromHeaderStrategy WithHeaders(params string[] headers) => new(headers);

    /// <summary>
    /// Creates a <see cref="TenantIdFromHeaderStrategy"/> with the provided headers and falls back to the <see cref="DefaultTenantIdHeader"/>.
    /// </summary>
    /// <param name="headers">The headers to first look for the <see cref="TenantId"/>.</param>
    /// <returns>The created <see cref="TenantIdStrategy"/>.</returns>
    public static TenantIdFromHeaderStrategy DefaultWithHeaders(params string[] headers) => new([.. headers, DefaultTenantIdHeader]);

    /// <summary>
    /// Initializes a new instance of the <see cref="TenantIdFromHeaderStrategy"/> class.
    /// </summary>
    /// <param name="headers">The headers to look for the </param>
    protected TenantIdFromHeaderStrategy(string[] headers)
    {
        _headers = headers;
    }

    /// <inheritdoc />
    protected override bool TryGet(HttpContext context, [NotNullWhen(true)]out TenantId? tenantId)
    {
        tenantId = null;
        var logger = GetLogger(context);
        foreach (var header in _headers)
        {
            LogTryingToGetTenantId(logger, header);
            if (!context.Request.Headers.TryGetValue(header, out var tenantIdString) || tenantIdString.Count == 0)
            {
                continue;
            }
            if (tenantIdString.Count == 1)
            {
                tenantId = tenantIdString.ToString();
                LogGotTenantId(logger, tenantId, header);
                return true;
            }
            tenantId = tenantIdString[0]!;
            LogMultipleValuesForHeader(logger, tenantId, header);
            return true;
        }
        return false;
    }

    [LoggerMessage(0, LogLevel.Debug, "Trying to get tenant id from header '{Header}'")]
    protected static partial void LogTryingToGetTenantId(ILogger logger, string header);

    [LoggerMessage(1, LogLevel.Debug, "Got tenant id '{TenantId}' from header '{Header}'")]
    protected static partial void LogGotTenantId(ILogger logger, TenantId tenantId, string header);

    [LoggerMessage(2, LogLevel.Warning, "Multiple values for header '{Header}'. Using the first tenant id '{TenantId}'")]
    protected static partial void LogMultipleValuesForHeader(ILogger logger, TenantId tenantId, string header);
}
