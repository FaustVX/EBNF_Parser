using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace EBNF_Parser.Core
{
    public class Parser
    {
        public void ParseText(string content)
        {

        }

        public static void ParseModel(string content)
        {
            var rules = Regex.Split(content, @"\s*;\s*(?:\r?\n)+")
                .Select(line => Regex.Replace(line, @"\s*(\r?\n)+\s*", " "))
                .Select(line => Regex.Match(line, @"(.*?)\s*=\s*(.*)").Groups)
                .ToDictionary(group => group[1].Value, SplitRHS(2));

            static Func<GroupCollection, Rule> SplitRHS(int index)
                => group => 
                {
                    var value = group[index];
                    return default!;
                };
        }
    }
}
