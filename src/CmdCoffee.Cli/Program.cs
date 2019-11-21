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

            while (args[0] != "q")
            {
                var output = "";
                var command = args[0];

                if (command == "help")
                {
                    output = commander.Help;
                }

                else if (!string.IsNullOrEmpty(command))
                {
                    output = commander.Execute(args);
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
                    var input = Console.ReadLine();
                    args = input?.Split(" ");
                } while (NeedInput());
            }
        }
    }

 }