﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace CmdCoffee.Cli
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Welcome to cmd.coffee.");
            var commandMapper = new CommandMapper(new ProductsCommand());
 
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

            if (NeedInput()) 
                GetInput();

            var commands = commandMapper.Commands;

            while (args[0] != "q")
            {
                var output = "";
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

                GetInput();
            }
        }
    }

 }