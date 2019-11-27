using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CmdCoffee.Cli
{
    public interface IOutputGenerator
    {
        string GenerateTable<T>(IEnumerable<T> source, string[] headers,
            params Func<T, object>[] valueSelectors);
  
        string GeneratePairs<T>(IEnumerable<KeyValuePair<string, T>> source, string title = default);
    }

    public class OutputGenerator : IOutputGenerator
    {
        public string GenerateTable<T>(IEnumerable<T> source, string[] headers, 
            params Func<T, object>[] valueSelectors)
        {
            return source.ToStringTable(headers, valueSelectors);
        }


        public string GeneratePairs<T>(IEnumerable<KeyValuePair<string, T>> source, string title = default)
        {
            if (source == null || source.Count() < 1)
                return string.Empty;

            var output = "\n";

            if (!string.IsNullOrEmpty(title))
                output += $"{title}:\n";

            var maxKeyLength = source.Max(s => s.Key.Length) + 2;

            foreach (var kvp in source)
            {
                var fixedLengthKey = string.Format("{0,-" + maxKeyLength + "}" , kvp.Key + ":");

                output += $"{fixedLengthKey} {kvp.Value}\n";
            }

            return output;
        }
    }
}