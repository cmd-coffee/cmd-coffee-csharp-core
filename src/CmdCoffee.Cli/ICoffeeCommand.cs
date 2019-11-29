using System.Collections.Generic;

namespace CmdCoffee.Cli
{
    public interface ICoffeeCommand 
    {
        string Name { get; }
        string Parameters { get; }
        string Description { get; }
        void Execute(IList<string> args);
    }
}