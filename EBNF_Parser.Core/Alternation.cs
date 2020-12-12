using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace EBNF_Parser.Core
{
    public class Alternation : IElement
    {
        public Alternation(IEnumerable<IElement> elements)
            : this(elements.ToArray())
        { }

        public Alternation(params IElement[] elements)
        {
            Elements = elements;
        }

        public IElement[] Elements { get; }

        public override string ToString()
            => string.Join<IElement>(" | ", Elements);

        public static bool TryParse(string input, [MaybeNullWhen(false)] out Alternation alternation)
        {
            var isOk = TryParse(input, out IElement? element);
            alternation = element as Alternation;
            return isOk;
        }

        internal static bool TryParse(string input, [MaybeNullWhen(false)] out IElement element)
            => IElement.TryParseMultiElem(input, @"\|", out element, (elem1, elem2) => new Alternation(elem1, elem2).Simplify());

        private Alternation Simplify()
        {
            if (Elements is {Length: 2} && Elements[1] is Alternation a)
                return new(a.Elements.Prepend(Elements[0]));
            return this;
        }
    }
}