using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace EBNF_Parser.Core
{
    public class Rule
    {
        public Rule (string identifier, IElement element)
        {
            Identifier = identifier;
            Element = element;
        }

        public string Identifier { get; }
        public IElement Element { get; }

        public static bool TryParse(string input, [MaybeNullWhen(false)] out Rule rule)
        {
            var match = Regex.Match(input, @$"^\s*({IElement.IdentifierPattern})\s*=\s*(.*?)(?:\s*;\s*)?\s*$");

            rule = default;
            return match is { Success: true, Groups: { Count: >= 3 } g }
                && IElement.TryParse(g[2].Value, out var elem)
                && ((rule = new Rule(g[1].Value, elem)) is not null);
        }
    }
}