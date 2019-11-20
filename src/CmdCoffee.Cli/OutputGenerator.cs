using System;
using System.Collections;
using System.Collections.Generic;

namespace CmdCoffee.Cli
{
    public interface IOutputGenerator
    {
        string GenerateTable<T>(IEnumerable<T> source, string[] headers,
            params Func<T, object>[] valueSelectors);
  
        string GeneratePairs(IDictionary<string, object> source);
    }

    public class OutputGenerator : IOutputGenerator
    {
        public string GenerateTable<T>(IEnumerable<T> source, string[] headers, 
            params Func<T, object>[] valueSelectors)
        {
            return source.ToStringTable(headers, valueSelectors);
        }

        public string GeneratePairs(IDictionary<string, object> source)
        {
            var output = "";

            foreach (var kvp in source)
            {
                output += $"\n{kvp.Key}:\t{kvp.Value}";
            }

            return output;
        }
    }
}