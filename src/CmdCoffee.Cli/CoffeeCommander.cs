using System;
using System.Collections.Generic;
using System.Linq;

namespace CmdCoffee.Cli
{
    public class CoffeeCommander
    {
        private readonly IOutputGenerator _outputGenerator;
        private readonly IOutputWriter _outputWriter;

        public CoffeeCommander(IOutputGenerator outputGenerator, IEnumerable<ICoffeeCommand> coffeeCommandsList, IOutputWriter outputWriter) 
        {
            _outputGenerator = outputGenerator;
            _outputWriter = outputWriter;
            var coffeeCommands = new Dictionary<string, ICoffeeCommand>();

            foreach (var coffeeCommand in coffeeCommandsList)
            {
                coffeeCommands[coffeeCommand.Name] = coffeeCommand;
            }

            CoffeeCommands = coffeeCommands;
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

        public void Execute(string[] args)
        {
            var command = args.FirstOrDefault();
            if (string.IsNullOrEmpty(command))
            {
                _outputWriter.WriteError("Command required");
                return;
            }

            if (CoffeeCommands.ContainsKey(command))
            {
                CoffeeCommands[command]?.Execute(args.Skip(1).Take(args.Length - 1).ToArray());
                return;
            }

            _outputWriter.WriteError($"No command found: {command}");
        }
    }
}