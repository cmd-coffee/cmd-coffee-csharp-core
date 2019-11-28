using System;
using System.Collections.Generic;
using System.Linq;
using CmdCoffee.Client;
using Newtonsoft.Json.Linq;

namespace CmdCoffee.Cli
{
    public class OrderCommand : ICoffeeCommand
    {
        private readonly ICmdCoffeeApi _cmdCoffeeApi;
        private readonly IOutputWriter _writer;
        private readonly IInputReader _reader;
        private readonly IOutputGenerator _outputGenerator;
        private readonly IAppSettings _appSettings;
        public string Name => "order";
        public string Parameters => "product-code [promo-code]";
        public string Description => "order 'product-code'";

        public OrderCommand(ICmdCoffeeApi cmdCoffeeApi, Func<IAppSettings> appSettingsFactory, 
            IOutputWriter writer, IInputReader reader, IOutputGenerator outputGenerator)
        {
            _cmdCoffeeApi = cmdCoffeeApi;
            _writer = writer;
            _reader = reader;
            _outputGenerator = outputGenerator;
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

                var result = _cmdCoffeeApi.PostOrder(productCode, _appSettings.ShippingAddress, promoCode).Result;

                _writer.WriteLine($"Your order has been accepted: orderKey: {result.orderDetails.orderKey}");
                
                _writer.WriteLine($"\nPlease confirm the shipping address: " +
                                  $"{_outputGenerator.GeneratePairs((IEnumerable<KeyValuePair<string,JToken>>)result.orderDetails.shippingAddress)}");

                _writer.WriteLine("Is it correct? (y/n)");
                if (_reader.ReadLine().ToLower() != "y")
                {
                    return "If your address is not correct, please update your address in app-settings.json and try again.";
                }
                    
                _writer.WriteLine($"Check out our return policy: {result.returnPolicy}");
                _writer.WriteLine("Does that work for you? (y/n)");
                if (_reader.ReadLine().ToLower() != "y")
                {
                    return $"Bummer. Feel free to shoot us an email to {_appSettings.ShippingAddress} to share your concerns.";
                }

                _writer.WriteLine($"Here are your payment options: {result.paymentOptions}");

                _writer.WriteLine($"We'll get started on your order as soon as we receive payment.");
                _writer.WriteLine($"Payment will be accepted until {result.paymentExpiration}");

                return string.Empty;

            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}