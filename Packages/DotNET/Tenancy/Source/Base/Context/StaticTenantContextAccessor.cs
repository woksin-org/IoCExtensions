// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Woksin.Extensions.Tenancy.Context;

/// <summary>
/// Represents static implementation of <see cref="ITenantContextAccessor{TTenant}"/> and <see cref="ITenantContextAccessor"/> that will be in use if AsyncLocal tenant context is disabled.
/// </summary>
/// <remarks>Unless <see cref="TenancyOptions{TTenant}.StaticTenantId"/> is configured this implementation will always return the 'unresolved' <see cref="ITenantContext"/>.</remarks>
/// <typeparam name="TTenant">The <see cref="Type"/> of the <see cref="ITenantInfo"/>.</typeparam>
public partial class StaticTenantContextAccessor<TTenant> : ITenantContextAccessor<TTenant>, ITenantContextAccessor
    where TTenant : class, ITenantInfo, new()
{
    readonly IOptionsMonitor<TenancyOptions<TTenant>> _options;
    readonly ILogger<StaticTenantContextAccessor<TTenant>> _logger;

    public StaticTenantContextAccessor(IOptionsMonitor<TenancyOptions<TTenant>> options, ILogger<StaticTenantContextAccessor<TTenant>> logger)
    {
        _options = options;
        _logger = logger;
    }

    /// <inheritdoc />
    public ITenantContext<TTenant> CurrentTenant
    {
        get
        {
            if (_options.CurrentValue.IsUsingStaticTenant(out var staticTenantId))
            {
                return TenantContext<TTenant>.Static(new TTenant
                {
                    Id = staticTenantId
                });
            }
            LogReturningUnresolvedContext(_logger);
            return TenantContext<TTenant>.Unresolved();
        }

        // ReSharper disable once ValueParameterNotUsed
        set
        {
            LogCannotSetContext(_logger);
        }
    }

    /// <inheritdoc />
    ITenantContext ITenantContextAccessor.CurrentTenant
    {
        get => CurrentTenant as ITenantContext ?? TenantContext<TTenant>.Unresolved();
        set => CurrentTenant = value as ITenantContext<TTenant> ?? throw new ArgumentNullException(nameof(value));
    }

    [LoggerMessage(LogLevel.Warning, "When tenant context is disabled it is not possible to set the tenant context")]
    static partial void LogCannotSetContext(ILogger logger);
    
    [LoggerMessage(LogLevel.Warning, "When tenant context is disabled and static tenant is not configured the tenant context will always be unresolved")]
    static partial void LogReturningUnresolvedContext(ILogger logger);
}
