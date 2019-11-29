using System;

namespace CmdCoffee.Cli
{
    public interface IOutputWriter
    {
        void WriteLine(string output = default);

        void AwaitAnyKey(string message = default);

        void WriteError(string errorMessage);

        void WriteError(Exception ex);

        bool AskYesNo(string question);
    }
}