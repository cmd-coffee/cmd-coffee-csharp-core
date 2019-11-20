using System;
using System.Collections.Generic;
using System.Linq;

namespace CmdCoffee.Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            var enterSelection = "Please enter selection. Type 'help' for options; 'q' to quit.";

            Console.WriteLine("Welcome to cmd.coffee.");
            Console.WriteLine(enterSelection);
            var commandMapper = new CommandMapper(new ProductsCommand());

            void GetInput()
            {
                do
                {
                    var input = System.Console.ReadLine();
                    args = input?.Split(" ");
                } while (args?.Length < 1);

            }

            if (args?.Length < 1) 
                GetInput();

            var commands = commandMapper.Commands;

            while (args[0] != "q")
            {
                string output = "";
                var command = args[0];

                if (command == "help")
                {
                    output = new OutputGenerator().GenerateTable(commands, new[] {"Command", "Description"},
                        kvp => $"{kvp.Value.Name} {kvp.Value.Parameters}", kvp => kvp.Value.Description);
                }

                else if (!string.IsNullOrEmpty(command))
                {
                    output = commands.ContainsKey(command) ? commands[command]?.Execute(args.Skip(1).Take(args.Length-1)) 
                        : $"No command found: {command}";
                }

                Console.WriteLine(output);

                Console.WriteLine(enterSelection);

                GetInput();
            }
        }
    }

 }