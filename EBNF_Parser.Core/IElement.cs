using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace EBNF_Parser.Core
{
    public interface IElement
    {
        string ToString();

        bool TryParse(string input, Parser parser, [MaybeNullWhen(false)] out int length);

        internal static string IdentifierPattern { get; } = "[a-zA-Z\\s_]+?";
        internal static string MultiElemPattern { get; } = @"\s*(.*?)\s*{0}\s*";
        internal static string GrouppingPattern { get; } = @"^\s*{0}\s*(.*?)\s*{1}\s*$";

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

        protected static bool TryParseGroupping(string input, string groupOpen, string groupClose, [MaybeNullWhen(false)] out IElement element, Func<IElement, IElement> ctor)
        {
            var match = Regex.Match(input, string.Format(IElement.GrouppingPattern, groupOpen, groupClose));

            element = default;
            return match is { Success: true, Groups: { Count: >= 2 } g }
                && IElement.TryParse(g[1].Value, out var elem)
                && ((element = ctor(elem)) is not null);
        }

        protected static bool TryParseGroupping(string input, string groupOpen, string groupClose, [MaybeNullWhen(false)] out IElement element, Func<string, IElement> ctor)
        {
            var match = Regex.Match(input, string.Format(IElement.GrouppingPattern, groupOpen, groupClose));

            element = default;
            return match is { Success: true, Groups: { Count: >= 2 } g }
                && g[1].Value is var elem
                && ((element = ctor(elem)) is not null);
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