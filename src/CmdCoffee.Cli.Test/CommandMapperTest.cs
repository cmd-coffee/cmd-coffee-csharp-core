using System.Collections.Generic;
using Castle.Core.Internal;
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

    public class OutputGeneratorTest
    {
        private readonly OutputGenerator _outputGenerator;

        public OutputGeneratorTest()
        {
            _outputGenerator = new OutputGenerator();
        }

        [Fact]
        public void GeneratePairs_EmptyDictionary_IsBlank()
        {
            var result = _outputGenerator.GeneratePairs(new Dictionary<string, object>(), "should not be used");
            result.Should().Be(string.Empty);
        }

        [Fact]
        public void GeneratePairs_NullDictionary_IsBlank()
        {
            var result = _outputGenerator.GeneratePairs(null, "should not be used");
            result.Should().Be(string.Empty);
        }

        [Fact]
        public void GeneratePairs_OnePair_HasTitleOnNewLineWithColon()
        {
            var result = _outputGenerator.GeneratePairs(new Dictionary<string, object> {{"one","value"}}, "should be used");
            result.IndexOf("\nshould be used:").Should().Be(0);
        }

        [Fact]
        public void GeneratePairs_OnePair_HasKeyValuesOnNewLine()
        {
            var result = _outputGenerator.GeneratePairs(new Dictionary<string, object> { { "one", "value" } }, "should be used");
            var lines = result.Split("\n");
            lines.Find(s => s == "one:  value").Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void GeneratePairs_TwoPairs_AlignsValues()
        {
            var result = _outputGenerator.GeneratePairs(new Dictionary<string, object>
                {
                    { "one", "value" },
                    {"much longer one", "value2" }
                }, 
                "should be used");
            var lines = result.Split("\n");
            lines.Find(s => s == "much longer one:  value2").Should().NotBeNullOrEmpty();

            lines.Find(s => s == "one:              value").Should().NotBeNullOrEmpty();
        }
    }
}
