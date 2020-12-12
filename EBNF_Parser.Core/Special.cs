using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace EBNF_Parser.Core
{
    public class Special : IElement
    {
        public Special(string value)
        {
            Value = value;
        }

        public string Value { get; }

        public override string ToString()
            => $"? {Value} ?";

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