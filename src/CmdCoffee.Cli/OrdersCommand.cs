using System.Collections.Generic;
using System.Linq;
using CmdCoffee.Client;

namespace CmdCoffee.Cli
{
    public class OrdersCommand : ICoffeeCommand
    {
        private readonly IOutputGenerator _outputGenerator;
        private readonly ICmdCoffeeApi _cmdCoffeeApi;
        public string Name => "orders";
        public string Parameters => "[order-key]";
        public string Description => "list your orders. specify order-key to see additional details";

        public OrdersCommand(IOutputGenerator outputGenerator, ICmdCoffeeApi cmdCoffeeApi)
        {
            _outputGenerator = outputGenerator;
            _cmdCoffeeApi = cmdCoffeeApi;
        }

        public string Execute(IList<string> args)
        {
            if (args == null || !args.Any())
            {
                var result = _cmdCoffeeApi.GetOrders().Result as IEnumerable<dynamic>;

                return _outputGenerator.GenerateTable(result, new[] { "order key", "product name", "status", "total (usd)", "date created" },
                    o => o.orderKey, o => o.productName, o => o.status, o => o.total, o => o.dateCreated);
            }
            else
            {
                var orderKey = args.FirstOrDefault();

                var order = _cmdCoffeeApi.GetOrder(orderKey).Result;

                if (order != null)
                { 
                    return _outputGenerator.GeneratePairs((IEnumerable < KeyValuePair < string, object >>) order);
                }

                return $"No order found: {orderKey}";
            }

        }
    }
}