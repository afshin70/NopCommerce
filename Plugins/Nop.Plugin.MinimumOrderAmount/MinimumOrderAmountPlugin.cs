﻿using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Nop.Core.Infrastructure;
using Nop.Services.Orders;
using Nop.Services.Plugins;

namespace Nop.Plugin.MinimumOrderAmount;

public class MinimumOrderAmountPlugin : BasePlugin, INopStartup
{
    public int Order => 3000;

    public void Configure(IApplicationBuilder application)
    {
     
    }

    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        var descriptor1 =new ServiceDescriptor(typeof(IOrderProcessingService),typeof(OrderProcessingService),ServiceLifetime.Scoped);
        services.Remove(descriptor1);
        services.AddScoped<IOrderProcessingService, CustomOrderProcessingService>();
        //var descriptor =new ServiceDescriptor(typeof(IOrderProcessingService),typeof(CustomOrderProcessingService),ServiceLifetime.Scoped);
        //services.Replace(descriptor);
    }

    public override string GetConfigurationPageUrl()
    {
        return "/Admin/MinimumOrderAmount/Configure";
    }
}