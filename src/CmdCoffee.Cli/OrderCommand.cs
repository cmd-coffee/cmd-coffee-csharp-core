using System;
using System.Collections.Generic;
using System.Linq;
using CmdCoffee.Client;

namespace CmdCoffee.Cli
{
    public class OrderCommand : ICoffeeCommand
    {
        private readonly ICmdCoffeeApi _cmdCoffeeApi;
        private readonly IAppSettings _appSettings;
        public string Name => "order";
        public string Parameters => "product-code [promo-code]";
        public string Description => "order 'product-code'";

        public OrderCommand(ICmdCoffeeApi cmdCoffeeApi, Func<IAppSettings> appSettingsFactory)
        {
            _cmdCoffeeApi = cmdCoffeeApi;
            _appSettings = appSettingsFactory();
        }

        public string Execute(IList<string> args)
        {
            try
            {
                var productCode = args.First();

                if (string.IsNullOrEmpty(productCode))
                {
                    return "product-code required";
                }

                var promoCode = args.Count() > 1 ? args[1] : string.Empty;

                if (_appSettings.ShippingAddress == null)
                {
                    return "shipping address required";
                }


                return _cmdCoffeeApi.PostOrder(productCode, _appSettings.ShippingAddress, promoCode).Result;


            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}