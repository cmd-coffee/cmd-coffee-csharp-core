using System;

namespace CmdCoffee.Cli
{
    public class ConsoleWrapper : IInputReader, IOutputWriter
    {
        public string ReadLine()
        {
            Console.Write("cmd.coffee> ");
            return Console.ReadLine();
        }

        public void WriteLine(string output)
        {
            Console.WriteLine(output);
        }
    }
}