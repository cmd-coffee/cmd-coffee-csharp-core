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
        private Mock<IOutputWriter> _mockOutputWriter;

        public CoffeeCommanderTest()
        {
            _mockOutputGenerator = new Mock<IOutputGenerator>();

            _mockOutputWriter = new Mock<IOutputWriter>();

            _mockCoffeeCommand = new Mock<ICoffeeCommand>();
            _mockCoffeeCommand.Setup(c => c.Name).Returns("first");

            _mockCoffeeCommand2 = new Mock<ICoffeeCommand>();
            _mockCoffeeCommand2.Setup(c => c.Name).Returns("second");

            _mockCoffeeCommand3 = new Mock<ICoffeeCommand>();
            _mockCoffeeCommand3.Setup(c => c.Name).Returns("third");

            var cc = new[]
            {
                _mockCoffeeCommand.Object,
                _mockCoffeeCommand2.Object,
                _mockCoffeeCommand3.Object
            };

            _coffeeCommander = new CoffeeCommander(_mockOutputGenerator.Object, cc, _mockOutputWriter.Object);
        }

        [Fact]
        public void CommandsList_NoCommands_ReturnsEmptyDictionary()
        {
            var commandMapper = new CoffeeCommander(_mockOutputGenerator.Object, new List<ICoffeeCommand>(),
                _mockOutputWriter.Object);
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
            _mockOutputGenerator.Setup(og => og.GenerateTable(_coffeeCommander.CoffeeCommands,
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
                commandOutputSelector.Invoke(_coffeeCommander.CoffeeCommands.First()).Should().Be("first [param1]");
            });

            var result = _coffeeCommander.Help;
        }

        [Fact]
        public void Help_CommandHasNoParameter_OnlyNameInOutput()
        {
            VerifyGenerateTable((commands, headers, valueSelectors) =>
            {
                var commandOutputSelector = valueSelectors[0];
                commandOutputSelector.Invoke(_coffeeCommander.CoffeeCommands.First()).ToString().Trim().Should().Be("first");
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
                descriptionSelector.Invoke(_coffeeCommander.CoffeeCommands.First()).Should().Be("does some stuff");
            });

            var result = _coffeeCommander.Help;
        }

        [Fact]
        public void Execute_CommandDoesNotExist_ReturnsMessage()
        {
            _coffeeCommander.Execute(new[] {"not-a-command"});
            _mockOutputWriter.Verify(w => w.WriteError("No command found: not-a-command"));
        }

        [Fact]
        public void Execute_CommandExistsNoAdditionalArgs_CallsExecuteWithEmptyArgs()
        {
            _coffeeCommander.Execute(new[] {"first"});

            _mockCoffeeCommand.Verify(cc => cc.Execute(It.Is<IList<string>>(args => !args.Any())));
        }

        [Fact]
        public void Execute_CommandExistsWithAdditionalArgs_CallsExecuteWithOtherArgs()
        {
            _coffeeCommander.Execute(new[] {"second", "arg2"});

            _mockCoffeeCommand2.Verify(cc => cc.Execute
                (It.Is<IList<string>>(args => args.Count(a => a == "arg2") == 1)));
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