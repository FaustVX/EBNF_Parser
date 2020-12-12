using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace EBNF_Parser.Core
{
    public class Rule
    {
        public Rule (string identifier, IElement element, Parser parser)
        {
            Identifier = identifier;
            Element = element;
            Parser = parser;
        }

        public string Identifier { get; }
        public IElement Element { get; }
        public Parser Parser { get; }

        public static bool TryParse(string input, [MaybeNullWhen(false)] out (string id, IElement elem) rule)
        {
            var match = Regex.Match(input, @$"^\s*({IElement.IdentifierPattern})\s*=\s*(.*?)(?:\s*;\s*)?\s*$");

            rule = default;
            return match is { Success: true, Groups: { Count: >= 3 } g }
                && IElement.TryParse(g[2].Value, out var elem)
                && ((rule = (g[1].Value, elem)) is not (null, null));
        }

        public bool TryParse(string input, [MaybeNullWhen(false)] out int length)
            => Element.TryParse(input, Parser, out length);
    }
}