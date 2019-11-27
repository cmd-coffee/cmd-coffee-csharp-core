using System.Collections.Generic;
using System.Linq;
using CmdCoffee.Client;

namespace CmdCoffee.Cli
{
    public class ProductsCommand : ICoffeeCommand
    {
        private readonly IOutputGenerator _outputGenerator;
        private readonly ICmdCoffeeApi _cmdCoffeeApi;
        public string Name => "products";
        public string Description => "list available coffees for order. specify product-code to see additional details";
        public string Parameters => "[product-code]";

        public ProductsCommand(IOutputGenerator outputGenerator, ICmdCoffeeApi cmdCoffeeApi)
        {
            _outputGenerator = outputGenerator;
            _cmdCoffeeApi = cmdCoffeeApi;
        }

        public string Execute(IList<string> args)
        {
            string output;

            if (args == null || !args.Any())
            {
                var result = _cmdCoffeeApi.GetProducts().Result as IEnumerable<dynamic>;

                output = _outputGenerator.GenerateTable(result, new[] {"code", "name", "price usd", "weight (oz)"},
                    p => p.code, p => p.name, p => p.priceUsd, p => p.weightInOunces);
            }
            else
            {
                var productCode = args.FirstOrDefault();

                var result = _cmdCoffeeApi.GetProducts().Result as IEnumerable<dynamic>;

                var product = result?.FirstOrDefault(p => p.code == productCode?.ToUpper());

                output = product != null
                    ? (string) _outputGenerator.GeneratePairs(product, "Product Details")
                    : $"no product found: {productCode}";
            }

            return output;
        }
    }
}