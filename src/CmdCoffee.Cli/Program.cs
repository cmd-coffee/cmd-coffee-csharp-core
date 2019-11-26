using CmdCoffee.Client;
using Microsoft.Extensions.DependencyInjection;

namespace CmdCoffee.Cli
{
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
            // add services
            serviceCollection.AddTransient<ICoffeeCommand, ProductsCommand>();
            serviceCollection.AddTransient<IOutputGenerator, OutputGenerator>();

            serviceCollection.AddTransient<CoffeeCommander>();

            serviceCollection.AddTransient<ICmdCoffeeApi, CmdCoffeeApi>();

            // add app
            serviceCollection.AddTransient<App>();
        }

    }

 }