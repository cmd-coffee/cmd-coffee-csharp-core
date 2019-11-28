using CmdCoffee.Client;

namespace CmdCoffee.Cli
{
    public class AppSettings : ICmdCoffeeApiSettings, IAppSettings
    {
        public string CmdCoffeeApiAddress { get; set; }
        public string AccessKey { get; set; }
        public ShippingAddress ShippingAddress { get; set; }

        public string ContactEmail => "support@cmd.coffee";
        public string PayPalAddress => "https://www.paypal.me/cmdcoffee/";
    }
}