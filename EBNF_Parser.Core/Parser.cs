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
            var identifier = Rules.Values.SelectMany(rule => SelectMany(rule.Element)).OfType<Identifier>().FirstOrDefault(elem => !Rules.ContainsKey(elem.Value));
            if (identifier is not null)
                throw new ArgumentOutOfRangeException("identifier", identifier, null);

            static IEnumerable<IElement> SelectMany(IElement element)
                => element switch
                {
                    Alternation { Elements: var elem } => elem,
                    Concatenation { Elements: var elem } => elem,
                    Exception { Elements: var elem } => elem,
                    Group { Value: var elem } => Enumerable.Repeat(elem, 1),
                    Option { Value: var elem } => Enumerable.Repeat(elem, 1),
                    Quantifier { Element: var elem } => Enumerable.Repeat(elem, 1),
                    Repetition { Element: var elem } => Enumerable.Repeat(elem, 1),
                    _ => Enumerable.Empty<IElement>()
                };
        }

        public bool TryParse(string content, [MaybeNullWhen(false)] Parsed parsed)
        {
            foreach (var rule in Rules.Values)
                if (rule.TryParse(content, out parsed))
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
