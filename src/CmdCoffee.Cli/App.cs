namespace CmdCoffee.Cli
{
    public class App
    {
        private readonly CoffeeCommander _coffeeCommander;
        private readonly IOutputWriter _outputWriter;
        private readonly IInputReader _inputReader;

        public App(CoffeeCommander coffeeCommander, IOutputWriter outputWriter, IInputReader inputReader)
        {
            _coffeeCommander = coffeeCommander;
            _outputWriter = outputWriter;
            _inputReader = inputReader;
        }

        public void Run(string[] args)
        {
            _outputWriter.WriteLine("Welcome to cmd.coffee.");

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

                _outputWriter.WriteLine(output);

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
                    _outputWriter.WriteLine(enterSelection);
                    var input = _inputReader.ReadLine();
                    args = input?.Split(" ");
                } while (NeedInput());
            }
        }

    }
}