using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.RegularExpressions;

namespace EBNF_Parser.Core
{
    public class Parser
    {
        public IReadOnlyDictionary<string, Rule> Rules { get; }

        private Parser((string id, IElement element)[] rules)
        {
            Rules = rules.ToDictionary(rule => rule.id, rule => new Rule(rule.id, rule.element, this));
        }

        public bool ParseText(string content)
        {
            foreach (var rule in Rules.Values)
                if (rule.TryParse(content, out _))
                    return true;
            return false;
        }

        public static Parser ParseModel(string content)
            => new(Regex.Split(content, @"\s*;\s*(?:\r?\n)+")
                .Select(line => Regex.Replace(line, @"\s*(\r?\n)+\s*", " "))
                .Select(line => Rule.TryParse(line, out var rule) ? rule : throw new(line))
                .ToArray());
    }
}
