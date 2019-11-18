using System.Collections;
using Castle.Core.Internal;
using FluentAssertions;
using Moq;
using Xunit;

namespace CmdCoffee.Console.Test
{
    public class CommandMapperTest
    {
        [Fact]
        public void CommandsList_CommandsNull_ReturnsEmptyDictionary()
        {
            var commandMapper = new CommandMapper(null);
            commandMapper.CommandsList.Should().BeEmpty();
        }

        [Fact]
        public void CommandsList_NoCommands_ReturnsEmptyDictionary()
        {
            var commandMapper = new CommandMapper();
            commandMapper.CommandsList.Should().BeEmpty();
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

            var commandsList = commandMapper.CommandsList;
            commandsList.Count.Should().Be(3);
            commandsList["first"].Should().Be("does some stuff");

            commandsList["second"].Should().Be("does some other stuff");

            commandsList["third"].Should().Be("does some fun stuff");
        }
    }
}
