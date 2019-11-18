namespace CmdCoffee.Console
{
    public interface ICoffeeCommand 
    {
        string Name { get; }
        string Description { get; }
    }
}