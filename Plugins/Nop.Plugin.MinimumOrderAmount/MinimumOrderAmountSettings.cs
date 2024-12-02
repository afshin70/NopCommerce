using Nop.Core.Configuration;

namespace Nop.Plugin.MinimumOrderAmount;

public class MinimumOrderAmountSettings : ISettings
{
    public decimal MinimumOrderAmount { get; set; }
}

public class MinimumOrderAmountModel
{
    public decimal MinimumOrderAmount { get; set; }
}
