using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace EBNF_Parser.Core
{
    public interface IElement
    {
        string ToString();

        internal static string IdentifierPattern { get; } = "[a-zA-Z\\s_]+";
        internal static string CharPattern { get; } = "[a-zA-Z0-9_&()<>{}=+*.,;:!?%$€#^" + /*/"]";//*/@"\|\-\[\]\/\s]";
        internal static string MultiElemPattern { get; } = @"\s*(.*?)\s*{0}\s*";

        protected static bool TryParseMultiElem(string input, string separator, [MaybeNullWhen(false)] out IElement element, Func<IElement, IElement, IElement> ctor)
        {
            var matches = Regex.Matches(input, string.Format(IElement.MultiElemPattern, separator));
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

        public static bool TryParse(string input, [MaybeNullWhen(false)] out IElement element)
            => Quantifier.TryParse(input, out element)
            || Exception.TryParse(input, out element)
            || Alternation.TryParse(input, out element)
            || Concatenation.TryParse(input, out element)

            // || Comment.TryParse(input, out element)
            || Group.TryParse(input, out element)
            || Option.TryParse(input, out element)
            || Repetition.TryParse(input, out element)
            || String.TryParse(input, out element)
            || Special.TryParse(input, out element)
            || Identifier.TryParse(input, out element)
            || false;
    }
}