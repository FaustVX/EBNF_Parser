using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace EBNF_Parser.Core
{
    public class Special : IElement
    {
        public Special(string value)
        {
            var group = Regex.Match(value, @"^(.+?)(?:\s+(.+))?$").Groups;
            Identifier = group[1].Value.Trim(' ');
            Parameter = group[2].Value.Trim(' ');
        }

        public string Identifier { get; }
        public string Parameter { get; }

        public override string ToString()
            => $"? {Identifier} {Parameter} ?";

        public bool TryParse(string input, Parser parser, [MaybeNullWhen(false)] out Parsed parsed)
            => (parser.Specials.TryGetValue(Identifier, out var parse) && parse(input, this, out parsed)) || (parsed = default) is not null;

        public static bool TryParse(string input, [MaybeNullWhen(false)] out Special special)
        {
            var isOk = TryParse(input, out IElement? element);
            special = element as Special;
            return isOk;
        }

        internal static bool TryParse(string input, [MaybeNullWhen(false)] out IElement element)
            => IElement.TryParseGroupping(input, @"\?", @"\?", out element, elem => new Special(elem));
    }
}