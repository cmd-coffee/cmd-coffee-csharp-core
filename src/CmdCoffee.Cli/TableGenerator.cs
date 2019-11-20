using System;
using System.Collections.Generic;

namespace CmdCoffee.Cli
{
    public interface ITableGenerator
    {
        string Generate<T>(IEnumerable<T> source, string[] headers,
            params Func<T, object>[] valueSelectors);
    }
    public class TableGenerator : ITableGenerator
    {
        public string Generate<T>(IEnumerable<T> source, string[] headers, 
            params Func<T, object>[] valueSelectors)
        {
            return source.ToStringTable(headers, valueSelectors);
        }
    }
}