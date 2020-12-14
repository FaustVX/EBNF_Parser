using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace EBNF_Parser.Core
{
    public class Identifier : IElement
    {
        public Identifier(string value)
        {
            Value = value;
        }

        public string Value { get; }

        public override string ToString()
            => Value;

        public bool TryParse(ReadOnlySpan<char> input, int start, Parser parser, [MaybeNullWhen(false)] out Parsed parsed)
            => (parser.Rules.TryGetValue(Value, out var elem) && elem.Element.TryParse(input, start, parser, out var p) && (parsed = p.With(this)) is not null) || (parsed = default) is not null;


        public static bool TryParse(string input, [MaybeNullWhen(false)] out Option option)
        {
            var isOk = TryParse(input, out IElement? element);
            option = element as Option;
            return isOk;
        }

        internal static bool TryParse(string input, [MaybeNullWhen(false)] out IElement element)
        {
            var match = Regex.Match(input, @$"^\s*({IElement.IdentifierPattern})\s*$");

            element = default;
            return match is { Success: true, Groups: { Count: >= 2 } g }
                && ((element = new Identifier(g[1].Value)) is not null);
        }
    }
}