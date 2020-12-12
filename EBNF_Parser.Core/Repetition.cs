using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace EBNF_Parser.Core
{
    public class Repetition : IElement
    {
        public Repetition(IElement element)
        {
            Element = element;
        }

        public IElement Element { get; }

        public override string ToString()
            => $"{{ {Element} }}";

        public bool TryParse(string input, Parser parser, [MaybeNullWhen(false)] out int length)
        {
            length = 0;
            while (true)
                if (!Element.TryParse(input[length..], parser, out var l))
                    break;
                else
                    length += l;
            return true;
        }

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