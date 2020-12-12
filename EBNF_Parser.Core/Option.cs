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

        public bool TryParse(string input, Parser parser, [MaybeNullWhen(false)] out int length)
            => Value.TryParse(input, parser, out length) || (length = 0) is not 0;

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