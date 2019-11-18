using System.Collections;
using System.Collections.Generic;
using FluentAssertions;
using Moq;
using Xunit;

namespace CmdCoffee.Console.Test
{
    public class CommandMapperTest
    {
        [Fact]
        public void CommandMapper_CommandsList_ReturnsDictionaryOfDescriptionsByNames()
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

    public class CommandMapper
    {
        private readonly ICoffeeCommand[] _coffeeCommands;

        public CommandMapper(params ICoffeeCommand[] coffeeCommands)
        {
            _coffeeCommands = coffeeCommands;
        }

        public IDictionary<string, string> CommandsList
        {
            get
            {
                var commandsList = new Dictionary<string, string>();
                foreach (var coffeeCommand in _coffeeCommands)
                {
                    commandsList[coffeeCommand.Name] = coffeeCommand.Description;
                }

                return commandsList;
            }

        }
    }

    public interface ICoffeeCommand 
    {
        string Name { get; }
        string Description { get; }
    }
}
