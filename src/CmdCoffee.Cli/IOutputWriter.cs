namespace CmdCoffee.Cli
{
    public interface IOutputWriter
    {
        void WriteLine(string output = default);

        void AwaitAnyKey(string message = default);

        void WriteError(string errorMessage);

        bool AskYesNo(string question);
    }
}