using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Samples.Configurations;
using Woksin.Extensions.Configurations.Tenancy;
using Woksin.Extensions.Tenancy;
using Woksin.Extensions.Tenancy.AspNetCore;
using Woksin.Extensions.Tenancy.AspNetCore.Strategies;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseTenantConfigurationExtension(typeof(Config).Assembly, _ =>
{
    _.ConfigureTenancy(_ => _.WithStrategy(QueryStrategy.Default), tenancyConfigurationPathParts: "Tenancy");
    
});
var app = builder.Build();
app.UseTenancy();
var options = app.Services.GetRequiredService<IOptions<Config>>();
var tenantOptions = app.Services.GetRequiredService<IOptions<TenantConfig>>();
var tenancyOptions = app.Services.GetRequiredService<IOptions<TenancyOptions<TenantInfo>>>();

app.MapGet("/", () => "Hello World!");
app.MapGet("/tenant", ([FromServices]IOptionsSnapshot<TenantConfig> options, [FromQuery]string tenantId) =>
{
    return options.Value;
});

app.Run();
