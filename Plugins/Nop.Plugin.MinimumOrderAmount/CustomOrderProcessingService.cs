using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Directory;
using Nop.Core.Domain.Localization;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Payments;
using Nop.Core.Domain.Shipping;
using Nop.Core.Domain.Tax;
using Nop.Core.Events;
using Nop.Core;
using Nop.Services.Affiliates;
using Nop.Services.Catalog;
using Nop.Services.Common;
using Nop.Services.Customers;
using Nop.Services.Directory;
using Nop.Services.Discounts;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Messages;
using Nop.Services.Orders;
using Nop.Services.Payments;
using Nop.Services.Security;
using Nop.Services.Shipping;
using Nop.Services.Stores;
using Nop.Services.Tax;
using Nop.Services.Vendors;

namespace Nop.Plugin.MinimumOrderAmount
{
    public class CustomOrderProcessingService : OrderProcessingService
    {
        private readonly MinimumOrderAmountSettings _settings;
        #region Fields

        protected readonly CurrencySettings _currencySettings;
        protected readonly IAddressService _addressService;
        protected readonly IAffiliateService _affiliateService;
        protected readonly ICheckoutAttributeFormatter _checkoutAttributeFormatter;
        protected readonly ICountryService _countryService;
        protected readonly ICurrencyService _currencyService;
        protected readonly ICustomerActivityService _customerActivityService;
        protected readonly ICustomerService _customerService;
        protected readonly ICustomNumberFormatter _customNumberFormatter;
        protected readonly IDiscountService _discountService;
        protected readonly IEncryptionService _encryptionService;
        protected readonly IEventPublisher _eventPublisher;
        protected readonly IGenericAttributeService _genericAttributeService;
        protected readonly IGiftCardService _giftCardService;
        protected readonly ILanguageService _languageService;
        protected readonly ILocalizationService _localizationService;
        protected readonly ILogger _logger;
        protected readonly IOrderService _orderService;
        protected readonly IOrderTotalCalculationService _orderTotalCalculationService;
        protected readonly IPaymentPluginManager _paymentPluginManager;
        protected readonly IPaymentService _paymentService;
        protected readonly IPdfService _pdfService;
        protected readonly IPriceCalculationService _priceCalculationService;
        protected readonly IPriceFormatter _priceFormatter;
        protected readonly IProductAttributeFormatter _productAttributeFormatter;
        protected readonly IProductAttributeParser _productAttributeParser;
        protected readonly IProductService _productService;
        protected readonly IReturnRequestService _returnRequestService;
        protected readonly IRewardPointService _rewardPointService;
        protected readonly IShipmentService _shipmentService;
        protected readonly IShippingService _shippingService;
        protected readonly IShoppingCartService _shoppingCartService;
        protected readonly IStateProvinceService _stateProvinceService;
        protected readonly IStoreMappingService _storeMappingService;
        protected readonly IStoreService _storeService;
        protected readonly ITaxService _taxService;
        protected readonly IVendorService _vendorService;
        protected readonly IWebHelper _webHelper;
        protected readonly IWorkContext _workContext;
        protected readonly IWorkflowMessageService _workflowMessageService;
        protected readonly LocalizationSettings _localizationSettings;
        protected readonly OrderSettings _orderSettings;
        protected readonly PaymentSettings _paymentSettings;
        protected readonly RewardPointsSettings _rewardPointsSettings;
        protected readonly ShippingSettings _shippingSettings;
        protected readonly TaxSettings _taxSettings;

        #endregion
        public CustomOrderProcessingService(
            MinimumOrderAmountSettings settings, CurrencySettings currencySettings,
        IAddressService addressService,
        IAffiliateService affiliateService,
        ICheckoutAttributeFormatter checkoutAttributeFormatter,
        ICountryService countryService,
        ICurrencyService currencyService,
        ICustomerActivityService customerActivityService,
        ICustomerService customerService,
        ICustomNumberFormatter customNumberFormatter,
        IDiscountService discountService,
        IEncryptionService encryptionService,
        IEventPublisher eventPublisher,
        IGenericAttributeService genericAttributeService,
        IGiftCardService giftCardService,
        ILanguageService languageService,
        ILocalizationService localizationService,
        ILogger logger,
        IOrderService orderService,
        IOrderTotalCalculationService orderTotalCalculationService,
        IPaymentPluginManager paymentPluginManager,
        IPaymentService paymentService,
        IPdfService pdfService,
        IPriceCalculationService priceCalculationService,
        IPriceFormatter priceFormatter,
        IProductAttributeFormatter productAttributeFormatter,
        IProductAttributeParser productAttributeParser,
        IProductService productService,
        IReturnRequestService returnRequestService,
        IRewardPointService rewardPointService,
        IShipmentService shipmentService,
        IShippingService shippingService,
        IShoppingCartService shoppingCartService,
        IStateProvinceService stateProvinceService,
        IStoreMappingService storeMappingService,
        IStoreService storeService,
        ITaxService taxService,
        IVendorService vendorService,
        IWebHelper webHelper,
        IWorkContext workContext,
        IWorkflowMessageService workflowMessageService,
        LocalizationSettings localizationSettings,
        OrderSettings orderSettings,
        PaymentSettings paymentSettings,
        RewardPointsSettings rewardPointsSettings,
        ShippingSettings shippingSettings,
        TaxSettings taxSettings)
            : base(currencySettings,
                  addressService,
                  affiliateService,
                  checkoutAttributeFormatter,
                  countryService,
                  currencyService,
                  customerActivityService,
                  customerService,
                  customNumberFormatter,
                  discountService,
                  encryptionService,
                  eventPublisher,
                  genericAttributeService,
                  giftCardService,
                  languageService,
                  localizationService,
                  logger,
                  orderService,
                  orderTotalCalculationService,
                  paymentPluginManager,
                  paymentService,
                  pdfService,
                  priceCalculationService,
                  priceFormatter,
                  productAttributeFormatter,
                  productAttributeParser,
                  productService,
                  returnRequestService,
                  rewardPointService,
                  shipmentService,
                  shippingService,
                  shoppingCartService,
                  stateProvinceService,
                  storeMappingService,
                  storeService,
                  taxService,
                  vendorService,
                  webHelper,
                  workContext,
                  workflowMessageService,
                  localizationSettings,
                  orderSettings,
                  paymentSettings,
                  rewardPointsSettings,
                  shippingSettings,
                  taxSettings)
        {
            _settings = settings;
            _currencySettings = currencySettings;
            _addressService = addressService;
            _affiliateService = affiliateService;
            _checkoutAttributeFormatter = checkoutAttributeFormatter;
            _countryService = countryService;
            _currencyService = currencyService;
            _customerActivityService = customerActivityService;
            _customerService = customerService;
            _customNumberFormatter = customNumberFormatter;
            _discountService = discountService;
            _encryptionService = encryptionService;
            _eventPublisher = eventPublisher;
            _genericAttributeService = genericAttributeService;
            _giftCardService = giftCardService;
            _languageService = languageService;
            _localizationService = localizationService;
            _logger = logger;
            _orderService = orderService;
            _orderTotalCalculationService = orderTotalCalculationService;
            _paymentPluginManager = paymentPluginManager;
            _paymentService = paymentService;
            _pdfService = pdfService;
            _priceCalculationService = priceCalculationService;
            _priceFormatter = priceFormatter;
            _productAttributeFormatter = productAttributeFormatter;
            _productAttributeParser = productAttributeParser;
            _productService = productService;
            _returnRequestService = returnRequestService;
            _rewardPointService = rewardPointService;
            _shipmentService = shipmentService;
            _shippingService = shippingService;
            _shoppingCartService = shoppingCartService;
            _stateProvinceService = stateProvinceService;
            _storeMappingService = storeMappingService;
            _storeService = storeService;
            _taxService = taxService;
            _vendorService = vendorService;
            _webHelper = webHelper;
            _workContext = workContext;
            _workflowMessageService = workflowMessageService;
            _localizationSettings = localizationSettings;
            _orderSettings = orderSettings;
            _paymentSettings = paymentSettings;
            _rewardPointsSettings = rewardPointsSettings;
            _shippingSettings = shippingSettings;
            _taxSettings = taxSettings;
        }

        public override async Task<PlaceOrderResult> PlaceOrderAsync(ProcessPaymentRequest processPaymentRequest)
        {
                Console.WriteLine("override PlaceOrderAsync called .............");
            // بررسی مبلغ سفارش قبل از ثبت
            var orderTotal = processPaymentRequest.OrderTotal;
            Console.WriteLine($"override PlaceOrderAsync called .............Total order is =>{orderTotal}");
            Console.WriteLine($"override PlaceOrderAsync called ............._settings.MinimumOrderAmount is =>{_settings.MinimumOrderAmount}");
            if (orderTotal < _settings.MinimumOrderAmount)
            {
                
                throw new Exception($"Order total must be at least {_settings.MinimumOrderAmount}.");
            }

            // ادامه فرآیند پیش‌فرض
            return await base.PlaceOrderAsync(processPaymentRequest);
        }
    }
}

