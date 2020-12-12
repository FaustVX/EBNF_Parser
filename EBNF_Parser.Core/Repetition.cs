using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace EBNF_Parser.Core
{
    public class Repetition : SingleElement
    {
        public Repetition(IElement element)
            : base(element)
        { }

        public override string ToString()
            => $"{{ {Element} }}";

        public override bool TryParse(string input, Parser parser, [MaybeNullWhen(false)] out Parsed parsed)
        {
            var length = 0;
            parsed = default;
            var list = new List<Parsed>();
            while (true)
            {
                if (!Element.TryParse(input[length..], parser, out var p))
                    break;
                length += p.Length;
                list.Add(p);
            }
            parsed = new(input[..length], this, length, list.ToArray());
            return true;
        }

        public static bool TryParse(string input, [MaybeNullWhen(false)] out Repetition repetition)
        {
            var isOk = TryParse(input, out IElement? element);
            repetition = element as Repetition;
            return isOk;
        }

        internal static bool TryParse(string input, [MaybeNullWhen(false)] out IElement element)
            => TryParse(input, @"{", @"}", out element, elem => new Repetition(elem));
    }
}