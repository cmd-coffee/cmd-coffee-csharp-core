using System;

namespace CmdCoffee.Cli
{
    public class ConsoleWrapper : IInputReader, IOutputWriter
    {
        public string ReadLine()
        {
            return Console.ReadLine();
        }

        public void WriteLine(string output)
        {
            Console.WriteLine(output);
        }
    }
}