using System.Collections.Generic;
using Castle.Core.Internal;
using FluentAssertions;
using Xunit;

namespace CmdCoffee.Cli.Test
{
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
            var result = _outputGenerator.GeneratePairs<object>(null, "should not be used");
            result.Should().Be(string.Empty);
        }

        [Fact]
        public void GeneratePairs_OnePair_HasTitleOnNewLineWithColon()
        {
            var result = _outputGenerator.GeneratePairs(new Dictionary<string, object> {{"one","value"}}, "should be used");
            result.IndexOf("\nshould be used:").Should().Be(0);
        }

        [Fact]
        public void GeneratePairs_NoTitle_HasNoTitleLine()
        {
            var result = _outputGenerator.GeneratePairs(new Dictionary<string, object> { { "one", "value" } });
            result.IndexOf("\n:").Should().Be(-1);
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