using System;
using System.Collections.Generic;
using System.Linq;
using CmdCoffee.Client;
using Newtonsoft.Json.Linq;

namespace CmdCoffee.Cli
{
    public class BuyCommand : ICoffeeCommand
    {
        private readonly ICmdCoffeeApi _cmdCoffeeApi;
        private readonly IOutputWriter _writer;
        private readonly IInputReader _reader;
        private readonly IOutputGenerator _outputGenerator;
        private readonly IAppSettings _appSettings;
        public string Name => "buy";
        public string Parameters => "product-code [promo-code]";
        public string Description => "place an order for one of our products";

        public BuyCommand(ICmdCoffeeApi cmdCoffeeApi, Func<IAppSettings> appSettingsFactory,
            IOutputWriter writer, IInputReader reader, IOutputGenerator outputGenerator)
        {
            _cmdCoffeeApi = cmdCoffeeApi;
            _writer = writer;
            _reader = reader;
            _outputGenerator = outputGenerator;
            _appSettings = appSettingsFactory();
        }

        public void Execute(IList<string> args)
        {
            try
            {
                var productCode = args.First();

                if (string.IsNullOrEmpty(productCode))
                {
                    _writer.WriteError("product-code required");
                    return;
                }

                var promoCode = args.Count() > 1 ? args[1] : string.Empty;

                if (_appSettings.ShippingAddress == null)
                {
                    _writer.WriteError("shipping address required");
                    return;
                }

                var result = _cmdCoffeeApi.PostOrder(productCode, _appSettings.ShippingAddress, promoCode).Result;

                var order = result.orderDetails;

                _writer.WriteLine(_outputGenerator.GeneratePairs(new Dictionary<string, object>
                {
                    {"product", order.productName},
                    {"sub-total", order.subTotal},
                    {"discount", order.discount},
                    {"total", order.total}
                }, "Please confirm order details"));

                _writer.WriteLine(
                    $"{_outputGenerator.GeneratePairs((IEnumerable<KeyValuePair<string, JToken>>) order.shippingAddress, "shipping address")}");

                if (!_writer.AskYesNo("Is this correct?"))
                {
                    _writer.WriteLine(
                        "If your address is not correct, please update your address in app-settings.json and try again.");
                    return;
                }

                _writer.WriteLine($"\nCheck out our return policy: {result.returnPolicy}");
                
                if (!_writer.AskYesNo("Does that work for you?"))
                {
                    _writer.AwaitAnyKey(
                        $"Bummer. Feel free to shoot us an email to {_appSettings.ContactEmail} to share your concerns.");
                    return;
                }

                _writer.WriteLine($"\nWe'll get started on your order as soon as we receive payment.");
                _writer.WriteLine($"Payment will be accepted until {result.paymentExpiration}");
                _writer.AwaitAnyKey("Press enter to see payment methods");
                
                _writer.WriteLine($"Send cryptocurrency payment to one of these addresses: {result.paymentOptions}");

                _writer.WriteLine($"or use this link {_appSettings.PayPalAddress}{order.total}USD");
                _writer.WriteLine($"Include your order key ({order.orderKey}) in the payment notes.");

                _writer.AwaitAnyKey();

            }
            catch (Exception ex)
            {
                _writer.WriteError(ex);
            }
        }
    }
}