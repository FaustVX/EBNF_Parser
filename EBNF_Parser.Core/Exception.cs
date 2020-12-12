using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace EBNF_Parser.Core
{
    public class Exception : IElement
    {
        public Exception(params IElement[] elements)
        {
            Elements = elements;
        }

        public Exception(IEnumerable<IElement> elements)
            : this(elements.ToArray())
        { }

        public IElement[] Elements { get; }

        public override string ToString()
            => string.Join<IElement>(" - ", Elements);

        public bool TryParse(string input, Parser parser, [MaybeNullWhen(false)] out Parsed parsed)
        {
            foreach (var element in Elements)
            {
                if (element.TryParse(input, parser, out parsed))
                    return true;
            }
            parsed = default;
            return false;
        }

        public static bool TryParse(string input, [MaybeNullWhen(false)] out Exception exception)
        {
            var isOk = TryParse(input, out IElement? element);
            exception = element as Exception;
            return isOk;
        }

        internal static bool TryParse(string input, [MaybeNullWhen(false)] out IElement element)
            => IElement.TryParseMultiElem(input, "-", out element, (elem1, elem2) => new Exception(elem1, elem2).Simplify());

        private Exception Simplify()
        {
            if (Elements is {Length: 2} && Elements[1] is Exception e)
                return new(e.Elements.Prepend(Elements[0]));
            return this;
        }
    }
}