using CmdCoffee.Cli;
using FluentAssertions;
using Xunit;

namespace CmdCoffee.Cli.Test
{
    public class ProductsCommandTest
    {
        [Fact]
        public void ImplementsICoffeeCommand()
        {
            (new ProductsCommand() as ICoffeeCommand).Should().NotBeNull();
        }
    }
}