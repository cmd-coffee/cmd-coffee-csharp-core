using System.Collections.Generic;

namespace CmdCoffee.Console
{
    public class CommandMapper
    {
        private readonly IEnumerable<ICoffeeCommand> _coffeeCommands;

        public CommandMapper(params ICoffeeCommand[] coffeeCommands)
        {
            if (coffeeCommands == null)
                coffeeCommands = new ICoffeeCommand[0];
            _coffeeCommands = coffeeCommands;
        }

        public IDictionary<string, ICoffeeCommand> Commands
        {
            get
            {
                var commandsList = new Dictionary<string, ICoffeeCommand>();
                foreach (var coffeeCommand in _coffeeCommands)
                {
                    commandsList[coffeeCommand.Name] = coffeeCommand;
                }

                return commandsList;
            }

        }
    }
}