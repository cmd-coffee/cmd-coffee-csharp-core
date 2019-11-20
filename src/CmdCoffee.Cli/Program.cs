using System;
using System.Linq;

namespace CmdCoffee.Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to cmd.coffee.");
            var commander = new CoffeeCommander(new OutputGenerator(), new ProductsCommand());

            if (NeedInput()) 
                GetInput();

            var commands = commander.CoffeeCommands;

            while (args[0] != "q")
            {
                var output = "";
                var command = args[0];

                if (command == "help")
                {
                    output = commander.Help;
//                    output = new OutputGenerator().GenerateTable(commands, new[] {"Command", "Description"},
//                        kvp => $"{kvp.Value.Name} {kvp.Value.Parameters}", kvp => kvp.Value.Description);
                }

                else if (!string.IsNullOrEmpty(command))
                {
                    output = commands.ContainsKey(command) ? commands[command]?.Execute(args.Skip(1).Take(args.Length-1)) 
                        : $"No command found: {command}";
                }

                Console.WriteLine(output);

                GetInput();
            }

            bool NeedInput()
            {
                return args?.Length < 1;
            }

            void GetInput()
            {
                const string enterSelection = "Please enter selection. Type 'help' for options; 'q' to quit.";

                do
                {
                    Console.WriteLine(enterSelection);
                    var input = System.Console.ReadLine();
                    args = input?.Split(" ");
                } while (NeedInput());
            }
        }
    }

 }