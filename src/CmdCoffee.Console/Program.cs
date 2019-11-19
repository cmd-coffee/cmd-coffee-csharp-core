namespace CmdCoffee.Console
{
    class Program
    {
        static void Main()
        {
            var enterSelection = "Please enter selection. Type 'help' for options; 'q' to quit.";

            System.Console.WriteLine("Welcome to cmd.coffee.");
            System.Console.WriteLine(enterSelection);
            var commandMapper = new CommandMapper(new ProductsCommand());

            var input = System.Console.ReadLine();

            var commands = commandMapper.Commands;
            while (input != "q")
            {
                if (input == "help")
                {
                    foreach (string key in commands.Keys)
                    {
                        var command = commands[key];
                        System.Console.WriteLine($"\n'{key}': {command.Description}\n");
                    }
                }

                else if (!string.IsNullOrEmpty(input))
                {
                    if (commands.ContainsKey(input))
                        commands[input]?.Execute();
                    else
                    {
                        System.Console.WriteLine($"No command found: {input}");
                    }
                }
                System.Console.WriteLine(enterSelection);
                input = System.Console.ReadLine();
            }
        }
    }

 }