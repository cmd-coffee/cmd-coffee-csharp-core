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

        public void WriteLine(string output = default)
        {
            Console.WriteLine(output);
        }

        public void AwaitAnyKey(string message = default)
        {
            if (!string.IsNullOrEmpty(message))
                Console.WriteLine(message);
            Console.WriteLine("Press enter to continue");
            Console.ReadLine();
        }

        public void WriteError(string errorMessage)
        {
            Console.WriteLine(errorMessage);
        }

        public void WriteError(Exception ex)
        {
            var aggregateException = ex as AggregateException;
            if (aggregateException != null)
            {
                if (aggregateException.InnerExceptions.Count == 1)
                { 
                    Console.WriteLine(ex.InnerException.Message);
                    return;
                }
            }
            Console.WriteLine(ex.Message);
        }

        public bool AskYesNo(string question)
        {
            Console.Write($"{question} (y/n)");
            var answer = (Console.ReadKey().KeyChar == 'y');
            Console.WriteLine();
            return answer;

        }
    }
}