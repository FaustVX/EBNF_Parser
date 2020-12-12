using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace EBNF_Parser.Core
{
    public class Concatenation : IElement
    {
        public Concatenation(IEnumerable<IElement> elements)
            : this (elements.ToArray())
        { }

        public Concatenation(params IElement[] elements)
        {
            Elements = elements;
        }

        public IElement[] Elements { get; }

        public override string ToString()
            => string.Join<IElement>(", ", Elements);

        public bool TryParse(string input, Parser parser, [MaybeNullWhen(false)] out int length)
        {
            length = 0;
            foreach (var element in Elements)
            {
                if (!element.TryParse(input[length..], parser, out var l))
                    return false;
                length += l;
            }
            return true;
        }

        public static bool TryParse(string input, [MaybeNullWhen(false)] out Concatenation concatenation)
        {
            var isOk = TryParse(input, out IElement? element);
            concatenation = element as Concatenation;
            return isOk;
        }

        internal static bool TryParse(string input, [MaybeNullWhen(false)] out IElement element)
            => IElement.TryParseMultiElem(input, ",", out element, (elem1, elem2) => new Concatenation(elem1, elem2).Simplify());

        private Concatenation Simplify()
        {
            if (Elements is {Length: 2} && Elements[1] is Concatenation c)
                return new(c.Elements.Prepend(Elements[0]));
            return this;
        }
    }
}