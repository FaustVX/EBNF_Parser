using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace EBNF_Parser.Core
{
    public class Alternation : MultiElement
    {
        public Alternation(IEnumerable<IElement> elements)
            : this(elements.ToArray())
        { }

        public Alternation(params IElement[] elements)
            : base(elements)
        { }

        public override string ToString()
            => string.Join<IElement>(" | ", Elements);

        public override bool TryParse(string input, Parser parser, [MaybeNullWhen(false)] out Parsed parsed)
        {
            foreach (var element in Elements)
                if (element.TryParse(input, parser, out parsed))
                    return true;
            parsed = default;
            return false;
        }

        public static bool TryParse(string input, [MaybeNullWhen(false)] out Alternation alternation)
        {
            var isOk = TryParse(input, out IElement? element);
            alternation = element as Alternation;
            return isOk;
        }

        internal static bool TryParse(string input, [MaybeNullWhen(false)] out IElement element)
            => TryParse(input, @"\|", out element, (elem1, elem2) => new Alternation(elem1, elem2).Simplify());

        private Alternation Simplify()
        {
            if (Elements is {Length: 2} && Elements[1] is Alternation a)
                return new(a.Elements.Prepend(Elements[0]));
            return this;
        }
    }
}