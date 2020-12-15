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

            if (match is not { Success: true, Groups: { Count: >= 3 } g })
                throw new InvalidRuleSyntax(input);
            rule = default;
            return IElement.TryParse(g[2].Value, out var elem)
                && ((rule = (g[1].Value, elem)) is not (null, null)) ? true : throw new InvalidRuleDefinitionSyntax(g[1].Value, g[2].Value);
        }

        public bool TryParse(string input, [MaybeNullWhen(false)] out Parsed parsed)
            => Element.TryParse(input, 0, Parser, out parsed) && (parsed = parsed?.With(default(Parsed)!)) is not null;
    }
}