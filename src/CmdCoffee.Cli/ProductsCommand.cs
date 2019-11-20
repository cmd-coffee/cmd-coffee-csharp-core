using System.Collections.Generic;
using System.Linq;
using CmdCoffee.Client;

namespace CmdCoffee.Cli
{

    public class ProductsCommand : ICoffeeCommand
    {
        private readonly ITableGenerator _tableGenerator;
        private readonly ICmdCoffeeApi _cmdCoffeeApi;
        public string Name => "products";
        public string Description => "list available coffees for order";

        public ProductsCommand() : this(new TableGenerator(), new CmdCoffeeApi())
        { }

        public ProductsCommand(ITableGenerator tableGenerator, ICmdCoffeeApi cmdCoffeeApi)
        {
            _tableGenerator = tableGenerator;
            _cmdCoffeeApi = cmdCoffeeApi;
        }

        public void Execute()
        {
            var result = _cmdCoffeeApi.GetProducts().Result as IEnumerable<dynamic>;

            var output = _tableGenerator.Generate(result, new[] { "Code", "Name", "Price USD", "Weight in Ounces" },
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
