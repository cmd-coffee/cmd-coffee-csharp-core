using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using Xunit;

namespace CmdCoffee.Cli.Test
{
    public class CoffeeCommanderTest
    {
        private readonly CoffeeCommander _coffeeCommander;
        private readonly Mock<IOutputGenerator> _mockOutputGenerator;
        private readonly Mock<ICoffeeCommand> _mockCoffeeCommand;
        private readonly Mock<ICoffeeCommand> _mockCoffeeCommand2;
        private readonly Mock<ICoffeeCommand> _mockCoffeeCommand3;
        private readonly Dictionary<string, ICoffeeCommand> _coffeeCommands;

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
            var commander =
                new CoffeeCommander(_mockOutputGenerator.Object, (IDictionary<string, ICoffeeCommand>) null);
            commander.CoffeeCommands.Should().BeEmpty();
        }

        [Fact]
        public void CommandsList_NoCommands_ReturnsEmptyDictionary()
        {
            var commandMapper = new CoffeeCommander(_mockOutputGenerator.Object, new List<ICoffeeCommand>());
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

        [Fact]
        public void Help_CommandHasParameter_IncludesParameterInOutput()
        {
            _mockCoffeeCommand.Setup(cc => cc.Parameters).Returns("[param1]");

            VerifyGenerateTable((commands, headers, valueSelectors) =>
            {
                var commandOutputSelector = valueSelectors[0];
                commandOutputSelector.Invoke(_coffeeCommands.First()).Should().Be("first [param1]");
            });

            var result = _coffeeCommander.Help;
        }

        [Fact]
        public void Help_CommandHasNoParameter_OnlyNameInOutput()
        {
            VerifyGenerateTable((commands, headers, valueSelectors) =>
            {
                var commandOutputSelector = valueSelectors[0];
                commandOutputSelector.Invoke(_coffeeCommands.First()).ToString().Trim().Should().Be("first");
            });

            var result = _coffeeCommander.Help;
        }

        [Fact]
        public void Help_CommandHasParameter_IncludesDescriptionInOutput()
        {
            _mockCoffeeCommand.Setup(c => c.Description).Returns("does some stuff");

            VerifyGenerateTable((commands, headers, valueSelectors) =>
            {
                var descriptionSelector = valueSelectors[1];
                descriptionSelector.Invoke(_coffeeCommands.First()).Should().Be("does some stuff");
            });

            var result = _coffeeCommander.Help;
        }

        [Fact]
        public void Execute_CommandDoesNotExist_ReturnsMessage()
        {
            _coffeeCommander.Execute(new[] {"not-a-command"}).Should().Be("No command found: not-a-command");
        }

        [Fact]
        public void Execute_CommandExistsNoAdditionalArgs_CallsExecuteWithEmptyArgs()
        {
            _mockCoffeeCommand.Setup(cc => cc.Execute(It.Is<IEnumerable<string>>(args => !args.Any())))
                .Returns("did some stuff");

            _coffeeCommander.Execute(new[] {"first"}).Should().Be("did some stuff");
        }

        [Fact]
        public void Execute_CommandExistsWithAdditionalArgs_CallsExecuteWithOtherArgs()
        {
            _mockCoffeeCommand2.Setup(cc => cc.Execute
                    (It.Is<IEnumerable<string>>(args => args.Count(a => a == "arg2") == 1)))
                .Returns("did some other stuff");

            _coffeeCommander.Execute(new[] {"second", "arg2"}).Should().Be("did some other stuff");
        }

        private void VerifyGenerateTable(
            Action<IEnumerable<KeyValuePair<string, ICoffeeCommand>>, string[],
                Func<KeyValuePair<string, ICoffeeCommand>, object>[]> verifyParams)
        {
            _mockOutputGenerator.Setup(og => og.GenerateTable(
                    It.IsAny<IEnumerable<KeyValuePair<string, ICoffeeCommand>>>(),
                    It.IsAny<string[]>(),
                    It.IsAny<Func<KeyValuePair<string, ICoffeeCommand>, object>[]>()))
                .Callback(verifyParams);
        }
    }
}