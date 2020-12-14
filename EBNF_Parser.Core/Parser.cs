using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace EBNF_Parser.Core
{
    public delegate bool TryParse(ReadOnlySpan<char> input, int start, Special element, [MaybeNullWhen(false)] out Parsed parser);

    public class Parser
    {
        private static readonly Dictionary<string, Parser> _parsers = new();

        public IReadOnlyDictionary<string, Rule> Rules { get; }
        public Dictionary<string, TryParse> Specials { get; } = new();

        private Parser((string id, IElement element)[] rules)
        {
            Rules = rules.ToDictionary(rule => rule.id, rule => new Rule(rule.id, rule.element, this));
            var identifier = Rules.Values.SelectMany(rule => SelectMany(rule, rule.Element)).OfType<Identifier>().FirstOrDefault(elem => !Rules.ContainsKey(elem.Value));
            if (identifier is not null)
                throw new UnreferencedIdentifierException(identifier.Value);

            static IEnumerable<IElement> SelectMany(Rule rule, IElement element)
                => (element switch
                {
                    MultiElement { Elements: var elem } => elem.SelectMany(e => SelectMany(rule, e)),
                    SingleElement { Element: var elem } => SelectMany(rule, elem),
                    Quantifier { Element: var elem } => SelectMany(rule, elem),
                    Identifier { Value: var value } when value == rule.Identifier => throw new CyclicReferenceException(value),
                    _ => Enumerable.Empty<IElement>()
                }).Prepend(element);
        }

        public bool TryParse(string content, [MaybeNullWhen(false)] out Parsed parsed)
        {
            foreach (var rule in Rules.Values)
                if (rule.TryParse(content, out parsed))
                    return true;
            parsed = default;
            return false;
        }

        public static bool TryParseFile(string filePath, string rule, [MaybeNullWhen(false)] out Parsed parsed)
            => (_parsers.TryGetValue(Path.GetExtension(filePath), out var parser)
                ? parser
                : (_parsers[Path.GetExtension(filePath)] = ParseModel(File.ReadAllText(Path.Combine(Path.GetDirectoryName(filePath) ?? "" ,Path.GetExtension(filePath).Trim('.') + ".ebnf")))))
                .Rules[rule].TryParse(File.ReadAllText(filePath), out parsed);

        public static Parser ParseModel(string content)
            => new(Regex.Split(content, @"\s*;\s*(?:\r?\n)+")
                .Select(line => Regex.Replace(line, @"\s*(\r?\n)+\s*", " "))
                .Select(line => Rule.TryParse(line, out var rule) ? rule : throw new())
                .ToArray());
    }
}
