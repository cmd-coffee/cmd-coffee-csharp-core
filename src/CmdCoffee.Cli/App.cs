using System;

namespace CmdCoffee.Cli
{
    public class App
    {
        private readonly CoffeeCommander _coffeeCommander;

        public App(CoffeeCommander coffeeCommander)
        {
            _coffeeCommander = coffeeCommander;
        }

        public void Run(string[] args)
        {
            Console.WriteLine("Welcome to cmd.coffee.");

            if (NeedInput())
                GetInput();

            while (args[0] != "q")
            {
                var output = "";
                var command = args[0];

                if (command == "help")
                {
                    output = _coffeeCommander.Help;
                }

                else if (!string.IsNullOrEmpty(command))
                {
                    output = _coffeeCommander.Execute(args);
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