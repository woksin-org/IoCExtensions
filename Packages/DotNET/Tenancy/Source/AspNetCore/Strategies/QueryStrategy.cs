// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Woksin.Extensions.Tenancy.Strategies;

namespace Woksin.Extensions.Tenancy.AspNetCore.Strategies;

/// <summary>
/// Represents an <see cref="ITenantResolutionStrategy"/> that resolves the tenant identifier by looking at the query parameters.
/// </summary>
public partial class QueryStrategy : HttpContextStrategy
{
    /// <summary>
    /// The default fallback query parameter to look for the tenant identifier in the query parameters.
    /// </summary>
    public const string DefaultQueryParameter = "TenantId";

    readonly string[] _queryParameters;

    /// <summary>
    /// The default implementation of <see cref="QueryStrategy"/>.
    /// </summary>
    public static readonly QueryStrategy Default = WithQueryParameters(DefaultQueryParameter);

    /// <summary>
    /// Creates a <see cref="QueryStrategy"/> with only the provided headers.
    /// </summary>
    /// <param name="queryParameters">The query parameters to look for the tenant identifier.</param>
    /// <returns>The created <see cref="QueryStrategy"/>.</returns>
    public static QueryStrategy WithQueryParameters(params string[] queryParameters) => new(queryParameters);

    /// <summary>
    /// Creates a <see cref="QueryStrategy"/> with the provided query parameters and falls back to the <see cref="DefaultQueryParameter"/>.
    /// </summary>
    /// <param name="queryParameters">The query parameters to first look for the tenant identifier.</param>
    /// <returns>The created <see cref="QueryStrategy"/>.</returns>
    public static QueryStrategy DefaultWithQueryParameters(params string[] queryParameters) => new([.. queryParameters, DefaultQueryParameter]);

    /// <summary>
    /// Initializes a new instance of the <see cref="QueryStrategy"/> class.
    /// </summary>
    /// <param name="queryParameters">The query parameters to look for the </param>
    protected QueryStrategy(string[] queryParameters)
    {
        _queryParameters = queryParameters;
    }

    /// <inheritdoc />
    protected override Task<string?> Resolve(HttpContext context, ILogger logger)
    {
        foreach (var queryParameter in _queryParameters)
        {
            LogTryingToGetTenantId(logger, queryParameter);
            if (!context.Request.Query.TryGetValue(queryParameter, out var tenantIdString) || tenantIdString.Count == 0)
            {
                continue;
            }
            string tenantId;
            if (tenantIdString.Count == 1)
            {
                tenantId = tenantIdString.ToString();
                LogGotTenantId(logger, tenantId, queryParameter);
                return Task.FromResult<string?>(tenantId);
            }
            tenantId = tenantIdString[0]!;
            LogMultipleValuesForHeader(logger, tenantId, queryParameter);
            return Task.FromResult<string?>(tenantId);
        }
        return Task.FromResult<string?>(null);
    }

    [LoggerMessage(0, LogLevel.Debug, "Trying to get tenant id from query parameter '{QueryParameter}'")]
    static partial void LogTryingToGetTenantId(ILogger logger, string queryParameter);

    [LoggerMessage(1, LogLevel.Debug, "Got tenant id '{TenantId}' from query parameter '{QueryParameter}'")]
    static partial void LogGotTenantId(ILogger logger, string tenantId, string queryParameter);

    [LoggerMessage(2, LogLevel.Warning, "Multiple values for query parameter '{QueryParameter}'. Using the first tenant id '{TenantId}'")]
    static partial void LogMultipleValuesForHeader(ILogger logger, string tenantId, string queryParameter);
}
