using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace EBNF_Parser.Core
{
    public class Repetition : IElement
    {
        public Repetition(IElement value)
        {
            Value = value;
        }

        public IElement Value { get; }

        public override string ToString()
            => $"{{ {Value} }}";

        public static bool TryParse(string input, [MaybeNullWhen(false)] out Repetition repetition)
        {
            var isOk = TryParse(input, out IElement? element);
            repetition = element as Repetition;
            return isOk;
        }

        internal static bool TryParse(string input, [MaybeNullWhen(false)] out IElement element)
            => IElement.TryParseGroupping(input, @"{", @"}", out element, elem => new Repetition(elem));
    }
}