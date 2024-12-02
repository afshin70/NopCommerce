using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Nop.Services.Configuration;

namespace Nop.Plugin.MinimumOrderAmount.Controllers;

[Area("Admin")]
public class MinimumOrderAmountController : Controller
{
    private readonly ISettingService _settingService;
    private readonly MinimumOrderAmountSettings _settings;

    public MinimumOrderAmountController(ISettingService settingService, IOptions<MinimumOrderAmountSettings> settings)
    {
        _settingService = settingService;
        _settings = settings.Value;
    }

    public IActionResult Configure()
    {
        var model = new MinimumOrderAmountModel
        {
            MinimumOrderAmount = _settings.MinimumOrderAmount
        };
        return View("~/Plugins/Nop.Plugin.MinimumOrderAmount/Views/Configure.cshtml", model);
    }

    [HttpPost]
    public IActionResult Configure(MinimumOrderAmountModel model)
    {
        if (!ModelState.IsValid)
            return Configure();

        _settings.MinimumOrderAmount = model.MinimumOrderAmount;
        _settingService.SaveSetting(_settings);

        TempData["success"] = "Settings saved successfully.";
        return RedirectToAction("Configure");
    }
}
