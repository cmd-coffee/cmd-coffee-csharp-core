using System.Collections.Generic;

namespace CmdCoffee.Cli
{
    public class CoffeeCommander
    {
        private readonly IOutputGenerator _outputGenerator;

        public CoffeeCommander(IOutputGenerator outputGenerator, params ICoffeeCommand[] coffeeCommandsList) 
        {
            _outputGenerator = outputGenerator;
            var coffeeCommands = new Dictionary<string, ICoffeeCommand>();
            foreach (var coffeeCommand in coffeeCommandsList)
            {
                coffeeCommands[coffeeCommand.Name] = coffeeCommand;
            }

            CoffeeCommands = coffeeCommands;
        }

        public CoffeeCommander(IOutputGenerator outputGenerator, IDictionary<string, ICoffeeCommand> coffeeCoffeeCommands)
        {
            _outputGenerator = outputGenerator;

            if (coffeeCoffeeCommands == null)
                coffeeCoffeeCommands = new Dictionary<string, ICoffeeCommand>();

            CoffeeCommands = coffeeCoffeeCommands;
        }

        public IDictionary<string, ICoffeeCommand> CoffeeCommands { get; }

        public string Help
        {
            get {
                return _outputGenerator.GenerateTable(CoffeeCommands, 
                    new string[] {"Command", "Description"},
                kvp=> $"{kvp.Value.Name} {kvp.Value.Parameters}", kvp=> kvp.Value.Description);

            }
        }
    }
}