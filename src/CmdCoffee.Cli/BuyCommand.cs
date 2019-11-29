using System;
using System.Collections.Generic;
using System.Linq;
using CmdCoffee.Client;
using Newtonsoft.Json.Linq;

namespace CmdCoffee.Cli
{
    public class InitCommand : ICoffeeCommand
    {
        private readonly ICmdCoffeeApi _cmdCoffeeApi;
        private readonly Func<ICmdCoffeeApiSettings> _apiSettingsFactory;
        private readonly IOutputWriter _outputWriter;
        private readonly IInputReader _inputReader;
        public string Name => "init";
        public string Parameters => "invite-code";
        public string Description => "request an access-key";

        public InitCommand(ICmdCoffeeApi cmdCoffeeApi, Func<ICmdCoffeeApiSettings> apiSettingsFactory,
            IOutputWriter outputWriter, IInputReader inputReader)
        {
            _cmdCoffeeApi = cmdCoffeeApi;
            _apiSettingsFactory = apiSettingsFactory;
            _outputWriter = outputWriter;
            _inputReader = inputReader;
        }

        public void Execute(IList<string> args)
        {
            var appSettings = _apiSettingsFactory();

            if (!string.IsNullOrEmpty(appSettings.AccessKey))
            {
                _outputWriter.WriteError("An access-key is already specified in your settings.");
                return;
            }

            var inviteCode = args.FirstOrDefault();

            if (string.IsNullOrEmpty(inviteCode))
            {
                _outputWriter.WriteError("'invite-code' is required");
                return;
            }

            var result = _cmdCoffeeApi.Join(inviteCode).Result;

            _outputWriter.WriteLine("\nInvite code accepted!");
            _outputWriter.WriteLine($"\n{result.legal}");
            _outputWriter.WriteLine($"{result.termsOfUse}"); 
            _outputWriter.WriteLine($"{result.privacyPolicy}"); 

            var answer = _outputWriter.AskYesNo("Ok?");

            if (!answer)
            {
                _outputWriter.AwaitAnyKey("Okay. When you change your mind, we're here for you");
                return;
            }

            _outputWriter.WriteLine($"\n{result.welcomeMessage}");

            _outputWriter.WriteLine($"access-key: {result.accessKey}");

            _outputWriter.WriteLine("\nTo use your accessKey, update your app-settings.json file");
            _outputWriter.WriteLine("Update your shipping address while you're at it!");

            _outputWriter.AwaitAnyKey();
        }

    }

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

                _writer.WriteLine("Is this correct? (y/n)");
                if (_reader.ReadLine().ToLower() != "y")
                {
                    _writer.AwaitAnyKey(
                        "If your address is not correct, please update your address in app-settings.json and try again.");
                    return;
                }

                _writer.WriteLine($"Check out our return policy: {result.returnPolicy}");
                _writer.WriteLine("Does that work for you? (y/n)");
                if (_reader.ReadLine().ToLower() != "y")
                {
                    _writer.AwaitAnyKey(
                        $"Bummer. Feel free to shoot us an email to {_appSettings.ContactEmail} to share your concerns.");
                    return;
                }

                _writer.WriteLine($"\nWe'll get started on your order as soon as we receive payment.");
                _writer.WriteLine($"Payment will be accepted until {result.paymentExpiration}");
                _writer.WriteLine("Press any key to see payment methods");
                _reader.ReadLine();

                _writer.WriteLine($"Send cryptocurrency payment to one of these addresses: {result.paymentOptions}");

                _writer.WriteLine($"or use this link {_appSettings.PayPalAddress}{order.total}USD");
                _writer.WriteLine($"Include your order key ({order.orderKey}) in the payment notes.");

                _writer.AwaitAnyKey();

                return;
            }
            catch (Exception ex)
            {
                _writer.WriteError(ex.Message);
                return;
            }
        }
    }
}