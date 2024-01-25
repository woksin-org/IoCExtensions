// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.Logging;

namespace Woksin.Extensions.Tenancy.Strategies;

/// <summary>
/// Represents a wrapper <see cref="ITenantResolutionStrategy"/> that safely performs the strategy while also logging.
/// </summary>
/// <param name="strategy">The <see cref="ITenantResolutionStrategy"/> to wrap.</param>
/// <param name="logger">The <see cref="ILogger"/> to use.</param>
public partial class SafeStrategyWrapper(ITenantResolutionStrategy strategy, ILogger logger) : ITenantResolutionStrategy
{
    public bool CanResolveFromContext(object context, out string cannotResolveReason)
    {
        try
        {
            return strategy.CanResolveFromContext(context, out cannotResolveReason);
        }
        catch (Exception e)
        {
            LogFailedCheckingCanResolve(logger, e);   
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<string?> Resolve(object resolutionContext)
    {
        try
        {
            LogTryResolve(logger, strategy.GetType());
            var identifier = await strategy.Resolve(resolutionContext);
            if (string.IsNullOrWhiteSpace(identifier))
            {
                LogNotResolved(logger, strategy.GetType());
            }
            else
            {
                LogResolved(logger, identifier, strategy.GetType());
            }

            return identifier;
        }
        catch (Exception ex)
        {
            LogError(logger, ex, strategy.GetType());
        }

        return null;
    }

    [LoggerMessage(0, LogLevel.Debug, "Tenant identifier {Identifier} was resolved from strategy {StrategyType}")]
    public static partial void LogResolved(ILogger logger, string identifier, Type strategyType);

    [LoggerMessage(1, LogLevel.Debug, "Tenant identifier was not resolved from strategy {StrategyType}")]
    public static partial void LogNotResolved(ILogger logger, Type strategyType);

    [LoggerMessage(2, LogLevel.Debug, "Error while resolving tenant identifier from strategy {StrategyType}")]
    public static partial void LogError(ILogger logger, Exception error, Type strategyType);

    [LoggerMessage(3, LogLevel.Debug, "Strategy {StrategyType} trying to resolve tenant identifier")]
    public static partial void LogTryResolve(ILogger logger, Type strategyType);
    
    [LoggerMessage(4, LogLevel.Warning, "Failed to check if could resolve from context")]
    public static partial void LogFailedCheckingCanResolve(ILogger logger, Exception ex);
}
