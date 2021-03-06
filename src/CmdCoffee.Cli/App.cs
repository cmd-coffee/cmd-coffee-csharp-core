﻿using System;

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
            {
                _outputWriter.WriteLine("Please enter selection. Type 'help' for options; 'q' to quit.");
                GetInput();
            }

            while (args[0] != "q")
            {
                try
                {
                    var command = args[0];

                    if (command == "help")
                    {
                        _outputWriter.WriteLine(_coffeeCommander.Help);
                    }

                    else if (!string.IsNullOrEmpty(command))
                    {
                        _coffeeCommander.Execute(args);
                    }

                }
                catch (Exception ex)
                {
                    _outputWriter.WriteError(ex);
                }

                GetInput();
            }

            bool NeedInput()
            {
                return args?.Length < 1;
            }

            void GetInput()
            {
                do
                {
                    var input = _inputReader.ReadLine();
                    args = input?.Split(" ");
                } while (NeedInput());
            }
        }

    }
}