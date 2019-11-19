namespace CmdCoffee.Cli
{
    public interface ICoffeeCommand 
    {
        string Name { get; }
        string Description { get; }
        void Execute();
    }
}