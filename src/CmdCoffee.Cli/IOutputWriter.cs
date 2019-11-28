namespace CmdCoffee.Cli
{
    public interface IOutputWriter
    {
        void WriteLine(string output = default);
    }
}