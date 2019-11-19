using FluentAssertions;
using Xunit;

namespace CmdCoffee.Console.Test
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