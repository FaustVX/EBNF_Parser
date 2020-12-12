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

        public bool TryParse(string input, Parser parser, [MaybeNullWhen(false)] out int length)
            => (parser.Rules.TryGetValue(Value, out var elem) && elem.Element.TryParse(input, parser, out length)) || (length = 0) is not 0;


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