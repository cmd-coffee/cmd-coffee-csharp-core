using System;
using System.Collections.Generic;
using FluentAssertions;
using Moq;
using Xunit;

namespace CmdCoffee.Cli.Test
{
    public class CoffeeCommanderTest
    {
        private CoffeeCommander _coffeeCommander;
        private Mock<IOutputGenerator> _mockOutputGenerator;
        private Mock<ICoffeeCommand> _mockCoffeeCommand;
        private Mock<ICoffeeCommand> _mockCoffeeCommand2;
        private Mock<ICoffeeCommand> _mockCoffeeCommand3;
        private Dictionary<string, ICoffeeCommand> _coffeeCommands;

        public CoffeeCommanderTest()
        {
            _mockOutputGenerator = new Mock<IOutputGenerator>();

            _mockCoffeeCommand = new Mock<ICoffeeCommand>();
            _mockCoffeeCommand.Setup(c => c.Name).Returns("first");

            _mockCoffeeCommand2 = new Mock<ICoffeeCommand>();
            _mockCoffeeCommand2.Setup(c => c.Name).Returns("second");

            _mockCoffeeCommand3 = new Mock<ICoffeeCommand>();
            _mockCoffeeCommand3.Setup(c => c.Name).Returns("third");

            _coffeeCommands = new Dictionary<string, ICoffeeCommand>
            {
                {"first", _mockCoffeeCommand.Object},
                {"second", _mockCoffeeCommand2.Object},
                {"third", _mockCoffeeCommand3.Object}
            };

            _coffeeCommander = new CoffeeCommander(_mockOutputGenerator.Object, _coffeeCommands);

        }

        [Fact]
        public void CommandsList_CommandsNull_ReturnsEmptyDictionary()
        {
            var commander = new CoffeeCommander(_mockOutputGenerator.Object, (IDictionary<string, ICoffeeCommand>)null);
            commander.CoffeeCommands.Should().BeEmpty();
        }

        [Fact]
        public void CommandsList_NoCommands_ReturnsEmptyDictionary()
        {
            var commandMapper = new CoffeeCommander(_mockOutputGenerator.Object);
            commandMapper.CoffeeCommands.Should().BeEmpty();
        }

        [Fact]
        public void CommandsList_CommandsExist_ReturnsDictionaryOfDescriptionsByNames()
        {
            _mockCoffeeCommand.Setup(c => c.Description).Returns("does some stuff");

            _mockCoffeeCommand2.Setup(c => c.Description).Returns("does some other stuff");

            _mockCoffeeCommand3.Setup(c => c.Description).Returns("does some fun stuff");

            var commandsList = _coffeeCommander.CoffeeCommands;
            commandsList.Count.Should().Be(3);
            commandsList["first"].Description.Should().Be("does some stuff");

            commandsList["second"].Description.Should().Be("does some other stuff");

            commandsList["third"].Description.Should().Be("does some fun stuff");
        }

        [Fact]
        public void Help_CommandsExist_UsesOutputGenerator()
        {
            _mockOutputGenerator.Setup(og => og.GenerateTable(_coffeeCommands,
                    new[] {"Command", "Description"},
                    It.IsAny<Func<KeyValuePair<string, ICoffeeCommand>, object>[]>()))
                .Returns("hi");

            _coffeeCommander.Help.Should().Be("hi");
        }

    }
}
