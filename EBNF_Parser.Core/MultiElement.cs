using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace EBNF_Parser.Core
{
    public abstract class MultiElement : IElement
    {
        private static string MultiElemPattern { get; } = @"\s*(.*?)\s*{0}\s*";

        public MultiElement(params IElement[] elements)
        {
            Elements = elements;
        }

        public IElement[] Elements { get; }

        public abstract bool TryParse(string input, Parser parser, [MaybeNullWhen(false)] out Parsed parsed);

        public abstract override string ToString();

        protected static bool TryParse(string input, string separator, [MaybeNullWhen(false)] out IElement element, Func<IElement, IElement, IElement> ctor)
        {
            var matches = Regex.Matches(input, string.Format(MultiElemPattern, separator));
            element = default;
            foreach (Match match in matches)
                if (match is { Success: true, Groups: { Count: >= 2 } g, Index: var idx, Length: var len }
                    && input[..(g[1].Index + g[1].Length)] is {} lhs
                    && input[(idx + len)..] is {} rhs
                    && IElement.TryParse(lhs, out var elem1)
                    && IElement.TryParse(rhs, out var elem2)
                    && ((element = ctor(elem1, elem2)) is not null))
                        return true;
            return false;
        }
    }
}