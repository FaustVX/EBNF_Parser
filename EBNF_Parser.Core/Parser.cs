using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace EBNF_Parser.Core
{
    public class Parser
    {
        public Rule[] Rules { get; }

        public Parser(Rule[] rules)
        {
            Rules = rules;
        }

        public void ParseText(string content)
        {

        }

        public static Parser ParseModel(string content)
            => new(Regex.Split(content, @"\s*;\s*(?:\r?\n)+")
                .Select(line => Regex.Replace(line, @"\s*(\r?\n)+\s*", " "))
                .Select(line => Rule.TryParse(line, out var rule) ? rule : throw new(line))
                .ToArray());
    }
}
