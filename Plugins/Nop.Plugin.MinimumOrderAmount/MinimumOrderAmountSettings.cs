using System.ComponentModel.DataAnnotations;
using Nop.Core.Configuration;

namespace Nop.Plugin.MinimumOrderAmount;

public class MinimumOrderAmountSettings : ISettings
{
    public decimal MinimumOrderAmount { get; set; }
}

public class MinimumOrderAmountModel
{
    [Display(Name = "Minimum Order Amount")]
    public decimal MinimumOrderAmount { get; set; }
}
