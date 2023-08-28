using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Woksin.Extensions.IoC.Autofac;
using Woksin.Extensions.IoC.Microsoft;

namespace Woksin.Extensions.IoC.Benchmarks;

[JsonExporterAttribute.FullCompressed]
public class BuildHost
{
    IHostBuilder _builder = null!;
    IHost _host = null!;

    [IterationSetup]
    public void Setup() => _builder = Host.CreateDefaultBuilder();

    [IterationCleanup]
    public void Cleanup()
    {
        _host.Dispose();
        _host = null!;
        _builder = null!;
    }

    [Benchmark]
    public ISingletonService MicrosoftIoC()
    {
        _builder.UseMicrosoftIoC(typeof(Program).Assembly,_ => _.IgnoredBaseTypes.Add(typeof(IPartiallyClosedGenericService<,>)));
        _host = _builder.Build();
        return _host.Services.GetRequiredService<ISingletonService>();
    }
    [Benchmark]
    public ISingletonService AutofacIoC()
    {
        _builder.UseAutofacIoC(typeof(Program).Assembly);
        _host = _builder.Build();
        return _host.Services.GetRequiredService<ISingletonService>();
    }
}


