using System;
using System.IO;
using CmdCoffee.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace CmdCoffee.Cli
{

    public class AppSettings : ICmdCoffeeApiSettings, IAppSettings
    {
        public string CmdCoffeeApiAddress { get; set; }
        public string AccessKey { get; set; }
        public ShippingAddress ShippingAddress { get; set; }
    }

    public class ShippingAddress
    {
        public string name { get; set; }
        public string company { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public string phone { get; set; }
        public string email { get; set; }

    }

    public interface IAppSettings
    {
        ShippingAddress ShippingAddress { get; set; }
    }

    class Program
    { 
        public static void Main(string[] args)
        {
            // create service collection
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            // create service provider
            var serviceProvider = serviceCollection.BuildServiceProvider();

            // entry to run app
            serviceProvider.GetService<App>().Run(args);
        }

        private static void ConfigureServices(IServiceCollection serviceCollection)
        {

            SetupConfiguration(serviceCollection);

            AddCommandServices(serviceCollection);

            AddIoHelpers(serviceCollection);

            AddApiClient(serviceCollection);

            serviceCollection.AddTransient<App>();
        }

        private static void SetupConfiguration(IServiceCollection serviceCollection)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("app-settings.json", optional: true, reloadOnChange: true)
                .Build();

            serviceCollection.AddOptions();

            serviceCollection.Configure<AppSettings>(configuration.GetSection("Configuration"));

            serviceCollection.AddTransient<Func<ICmdCoffeeApiSettings>>(x =>
                () => x.GetService<IOptions<AppSettings>>().Value);

            serviceCollection.AddTransient<Func<IAppSettings>>(x =>
                () => x.GetService<IOptions<AppSettings>>().Value);

        }

        private static void AddApiClient(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<ICmdCoffeeApi, CmdCoffeeApi>();

        }

        private static void AddIoHelpers(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IOutputGenerator, OutputGenerator>();
            serviceCollection.AddTransient<IOutputWriter, ConsoleWrapper>();
            serviceCollection.AddTransient<IInputReader, ConsoleWrapper>();
        }

        private static void AddCommandServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<ICoffeeCommand, ProductsCommand>();
            serviceCollection.AddTransient<ICoffeeCommand, OrderCommand>();
            serviceCollection.AddTransient<CoffeeCommander>();
        }
    }

 }