// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.ComponentModel;
using System.Globalization;

namespace Woksin.Extensions.IoC.Tenancy;

/// <summary>
/// Represents a <see cref="TypeConverter"/> for handling conversion of <see cref="TenantId"/>.
/// </summary>
public class TenantIdTypeConverter : TypeConverter
{
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        => sourceType == typeof(string);

    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
        => TenantId.FromString((string)value);
}
