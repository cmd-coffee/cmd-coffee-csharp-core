using FluentAssertions;
using Moq;
using Xunit;

namespace CmdCoffee.Cli.Test
{
    public class CommandMapperTest
    {
        [Fact]
        public void CommandsList_CommandsNull_ReturnsEmptyDictionary()
        {
            var commandMapper = new CommandMapper(null);
            commandMapper.Commands.Should().BeEmpty();
        }

        [Fact]
        public void CommandsList_NoCommands_ReturnsEmptyDictionary()
        {
            var commandMapper = new CommandMapper();
            commandMapper.Commands.Should().BeEmpty();
        }

        [Fact]
        public void CommandsList_CommandsExist_ReturnsDictionaryOfDescriptionsByNames()
        {
            var mockCoffeeCommand = new Mock<ICoffeeCommand>();
            mockCoffeeCommand.Setup(c => c.Name).Returns("first");
            mockCoffeeCommand.Setup(c => c.Description).Returns("does some stuff");

            var mockCoffeeCommand2 = new Mock<ICoffeeCommand>();
            mockCoffeeCommand2.Setup(c => c.Name).Returns("second");
            mockCoffeeCommand2.Setup(c => c.Description).Returns("does some other stuff");

            var mockCoffeeCommand3 = new Mock<ICoffeeCommand>();
            mockCoffeeCommand3.Setup(c => c.Name).Returns("third");
            mockCoffeeCommand3.Setup(c => c.Description).Returns("does some fun stuff");

            var commandMapper = new CommandMapper(mockCoffeeCommand.Object, mockCoffeeCommand2.Object, mockCoffeeCommand3.Object);

            var commandsList = commandMapper.Commands;
            commandsList.Count.Should().Be(3);
            commandsList["first"].Description.Should().Be("does some stuff");

            commandsList["second"].Description.Should().Be("does some other stuff");

            commandsList["third"].Description.Should().Be("does some fun stuff");
        }

        [Fact]
        public void CommandList_CommandsExist_ReturnsNameDescriptionPairs()
        {

        }
    }
}
