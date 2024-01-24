// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Woksin.Extensions.Tenancy.Strategies;

/// <summary>
/// Represents the information about the
/// </summary>
/// <param name="StrategyType">The clr <see cref="Type"/> of the <see cref="ITenantResolutionStrategy"/> used.</param>
/// <param name="Strategy">The <see cref="ITenantResolutionStrategy"/> used.</param>
public record StrategyInfo(Type StrategyType, ITenantResolutionStrategy Strategy);
