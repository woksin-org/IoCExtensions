// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using Perfolizer.Horology;

namespace Woksin.Extensions.IoC.Benchmarks;

public static class Program
{
    public static void Main(string[] args)
    {
        BenchmarkRunner.Run(typeof(Program).Assembly, new ManualConfig()
        {
            SummaryStyle = SummaryStyle.Default
                .WithRatioStyle(RatioStyle.Percentage)
                .WithSizeUnit(SizeUnit.KB)
                .WithTimeUnit(TimeUnit.Millisecond),
        }, args);
    }
}
