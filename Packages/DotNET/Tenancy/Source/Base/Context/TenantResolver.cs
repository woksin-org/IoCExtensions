// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Woksin.Extensions.Tenancy.Strategies;

namespace Woksin.Extensions.Tenancy.Context;

/// <summary>
/// Represents an implementation of <see cref="IResolveTenant"/> and <see cref="IResolveTenant{TTenant}"/> that resolves tenant context using the configured strategies.
/// </summary>
/// <param name="strategies">The <see cref="ITenantResolutionStrategy"/> strategies to resolve tenant identifier.</param>
/// <param name="options">The <see cref="IOptionsMonitor{TOptions}"/> for the configured <see cref="TenantsConfigurationOption{TTenant}"/>.</param>
/// <param name="loggerFactory">The <see cref="ILoggerFactory"/>.</param>
/// <typeparam name="TTenant">The <see cref="Type"/> of the <see cref="ITenantInfo"/>.</typeparam>
public partial class TenantResolver<TTenant>(IEnumerable<ITenantResolutionStrategy> strategies, IOptionsMonitor<TenantsConfigurationOption<TTenant>> options, ILoggerFactory loggerFactory) : IResolveTenant<TTenant>, IResolveTenant
    where TTenant : class, ITenantInfo, new()
{
    /// <inheritdoc />
    public IEnumerable<ITenantResolutionStrategy> Strategies { get; } = strategies;

    readonly ILogger<TenantResolver<TTenant>> _logger = loggerFactory?.CreateLogger<TenantResolver<TTenant>>() ?? NullLogger<TenantResolver<TTenant>>.Instance;

    /// <inheritdoc />
    public async Task<ITenantContext<TTenant>> Resolve(object context)
    {
        var config = options.CurrentValue;
        foreach (var strategy in Strategies)
        {
            var (resolvedIdentifier, identifier) = await TryResolveIdentifier(config, strategy, context);
            if (resolvedIdentifier && TryGetTenantContext(config, identifier!, strategy, out var tenantContext))
            {
                return tenantContext;
            }
        }

        LogCouldNotResolveTenant(_logger);
        return TenantContext<TTenant>.Unresolved();
    }

    async Task<(bool, string?)> TryResolveIdentifier(TenantsConfigurationOption<TTenant> config, ITenantResolutionStrategy strategy, object context)
    {
        var wrappedStrategy = new SafeStrategyWrapper(strategy, loggerFactory?.CreateLogger(strategy.GetType()) ?? NullLogger.Instance);
        var identifier = await wrappedStrategy.Resolve(context);
        if (!config.Ignored.Contains(identifier, StringComparer.OrdinalIgnoreCase))
        {
            return string.IsNullOrWhiteSpace(identifier)
                ? (false, null)
                : (true, identifier);
        }
        LogIgnoreTenant(_logger, identifier!);
        return (false, null);
    }

    bool TryGetTenantContext(TenantsConfigurationOption<TTenant> config, string identifier, ITenantResolutionStrategy strategy, [NotNullWhen(true)]out ITenantContext<TTenant>? tenantContext)
    {
        tenantContext = null;
        var configuredTenant = config.Tenants.FirstOrDefault(tenant => tenant.Id.Equals(identifier, StringComparison.OrdinalIgnoreCase));
        if (configuredTenant is not null)
        {
            LogUsingConfiguredTenant(_logger, identifier, configuredTenant.Name);
            tenantContext = TenantContext<TTenant>.Resolved(configuredTenant, new StrategyInfo(strategy.GetType(), strategy));
            return true;
        }

        if (config.Strict)
        {
            LogTenantNotConfigured(_logger, identifier);
            return false;
        }
        LogUsingNonConfiguredTenant(_logger, identifier);
        configuredTenant = new TTenant
        {
            Id = identifier
        };
        tenantContext = TenantContext<TTenant>.Resolved(configuredTenant, new StrategyInfo(strategy.GetType(), strategy));
        return true;
    }

    async Task<ITenantContext> IResolveTenant.Resolve(object context)
        => await Resolve(context) as ITenantContext ?? throw new InvalidCastException("Could not cast tenant context");

    [LoggerMessage(0, LogLevel.Debug, "Resolved tenant identifier {Identifier} is configured to be ignored")]
    static partial void LogIgnoreTenant(ILogger logger, string identifier);

    [LoggerMessage(1, LogLevel.Debug, "Resolved tenant identifier {Identifier} is not configured and should not be used")]
    static partial void LogTenantNotConfigured(ILogger logger, string identifier);

    [LoggerMessage(2, LogLevel.Debug, "Resolved tenant identifier {Identifier} is not configured but will be used")]
    static partial void LogUsingNonConfiguredTenant(ILogger logger, string identifier);

    [LoggerMessage(3, LogLevel.Debug, "Resolved tenant identifier {Identifier} with name {TenantName} is configured")]
    static partial void LogUsingConfiguredTenant(ILogger logger, string identifier, string? tenantName);

    [LoggerMessage(4, LogLevel.Warning, "Could not resolve tenant from any strategy")]
    static partial void LogCouldNotResolveTenant(ILogger logger);
}
