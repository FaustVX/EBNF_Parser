using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace EBNF_Parser.Core
{
    public class Option : IElement
    {
        public Option(IElement value)
        {
            Value = value;
        }

        public IElement Value { get; }

        public override string ToString()
            => $"[ {Value} ]";

        public bool TryParse(string input, Parser parser, [MaybeNullWhen(false)] out Parsed parsed)
            => (Value.TryParse(input, parser, out var p) && (parsed = p.With(this)) is not null) || (parsed = default) is not null;

        public static bool TryParse(string input, [MaybeNullWhen(false)] out Option option)
        {
            var isOk = TryParse(input, out IElement? element);
            option = element as Option;
            return isOk;
        }

        internal static bool TryParse(string input, [MaybeNullWhen(false)] out IElement element)
            => IElement.TryParseGroupping(input, @"\[", @"\]", out element, elem => new Option(elem));
    }
}