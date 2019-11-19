using System.Collections.Generic;
using System.Linq;
using CmdCoffee.Client;

namespace CmdCoffee.Cli
{

    public class ProductsCommand : ICoffeeCommand
    {
        public string Name => "products";
        public string Description => "list available coffees for order";
        public string Params => string.Empty;

        public void Execute()
        {
            var result = CmdCoffeeApi.GetProducts().Result as IEnumerable<dynamic>;
            var output = result.ToStringTable(
                new[] { "Code", "Name", "Price USD", "Weight in Ounces" },
                p => p.code, p => p.name, p => p.priceUsd, p => p.weightInOunces);
            System.Console.WriteLine(output);

            var directions = "enter 'code' for details or 'b' to go back";
            System.Console.WriteLine(directions);

            var input = System.Console.ReadLine();

            while (input != "b")
            {
                var product = (IDictionary<string, object>)result.FirstOrDefault(p => p.code == input.ToUpper());

                if (product == null)
                    System.Console.WriteLine($"no product found: {input}");

                else
                {
                    foreach (string key in product.Keys)
                        System.Console.WriteLine($"{key}:\t{product[key]}");
                }
                System.Console.WriteLine();
                System.Console.WriteLine(directions);
                input = System.Console.ReadLine();
            }
        }
    }
}
