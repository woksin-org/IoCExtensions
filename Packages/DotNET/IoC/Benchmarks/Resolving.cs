using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Woksin.Extensions.IoC.Autofac;
using Woksin.Extensions.IoC.Microsoft;

namespace Woksin.Extensions.IoC.Benchmarks;

[JsonExporterAttribute.FullCompressed]
public class Resolving
{
    IHostBuilder _microsoftBuilder = null!;
    IHostBuilder _autofacBuilder = null!;
    IHost _microsoftHost = null!;
    IHost _autofacHost = null!;


    [Params(1, 10, 100)]
    public int TimesToResolve { get; set;}

    [IterationSetup]
    public void Setup()
    {
        _microsoftBuilder = Host.CreateDefaultBuilder().UseMicrosoftIoC(typeof(Program).Assembly,_ => _.IgnoredBaseTypes.Add(typeof(IPartiallyClosedGenericService<,>)));
        _autofacBuilder = Host.CreateDefaultBuilder().UseAutofacIoC(typeof(Program).Assembly);

        _microsoftHost = _microsoftBuilder.Build();
        _autofacHost = _autofacBuilder.Build();
    }

    [IterationCleanup]
    public void Cleanup()
    {
        _microsoftHost.Dispose();
        _autofacHost.Dispose();
        _microsoftHost = null!;
        _autofacHost = null!;
        _microsoftBuilder = null!;
        _autofacBuilder = null!;
    }

    [Benchmark]
    public ITransientService[] MicrosoftIoC() => DoTest(TimesToResolve, _microsoftHost);

    [Benchmark]
    public ITransientService[] AutofacIoC() => DoTest(TimesToResolve, _autofacHost);

    public static ITransientService[] DoTest(int timesToResolve, IHost host)
    {
        var result = new ITransientService[timesToResolve];
        for (var i = 0; i < timesToResolve; i++)
        {
            result[i] = host.Services.GetRequiredService<ITransientService>();
        }

        return result;
    }
}


