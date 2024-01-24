// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Woksin.Extensions.Configurations;
using Woksin.Extensions.Configurations.Tenancy;

namespace Samples.Configurations;

[Configuration("Config")]
public class Config
{
    public string Value { get; set; }
}

[TenantConfiguration("Config")]
public class TenantConfig
{
    public string Value { get; set; }
}
