using System.Collections.Generic;
using System.Linq;
using CmdCoffee.Client;

namespace CmdCoffee.Cli
{
    public class OrdersCommand : ICoffeeCommand
    {
        private readonly IOutputGenerator _outputGenerator;
        private readonly ICmdCoffeeApi _cmdCoffeeApi;
        private readonly IOutputWriter _outputWriter;
        public string Name => "orders";
        public string Parameters => "[order-key]";
        public string Description => "list your orders. specify order-key to see additional details";

        public OrdersCommand(IOutputGenerator outputGenerator, ICmdCoffeeApi cmdCoffeeApi, IOutputWriter outputWriter)
        {
            _outputGenerator = outputGenerator;
            _cmdCoffeeApi = cmdCoffeeApi;
            _outputWriter = outputWriter;
        }

        public void Execute(IList<string> args)
        {
            if (args == null || !args.Any())
            {
                var result = _cmdCoffeeApi.GetOrders().Result as IEnumerable<dynamic>;

                _outputWriter.WriteLine(_outputGenerator.GenerateTable(result, new[] { "order key", "product name", "status", "total (usd)", "date created" },
                    o => o.orderKey, o => o.productName, o => o.status, o => o.total, o => o.dateCreated));
            }
            else
            {
                var orderKey = args.FirstOrDefault();

                var order = _cmdCoffeeApi.GetOrder(orderKey).Result;

                if (order != null)
                {
                    _outputWriter.WriteLine(_outputGenerator.GeneratePairs((IEnumerable < KeyValuePair < string, object >>) order));
                    return;
                }

                _outputWriter.WriteError($"No order found: {orderKey}");
            }

        }
    }
}