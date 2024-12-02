using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nop.Core.Infrastructure;
using Nop.Services.Orders;
using Nop.Services.Plugins;

namespace Nop.Plugin.MinimumOrderAmount;

public class MinimumOrderAmountPlugin : BasePlugin, INopStartup
{
    public int Order => 500;

    public void Configure(IApplicationBuilder application)
    {
     
    }

    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IOrderProcessingService, CustomOrderProcessingService>();
    }

    public override string GetConfigurationPageUrl()
    {
        return "/Admin/MinimumOrderAmount/Configure";
    }
}