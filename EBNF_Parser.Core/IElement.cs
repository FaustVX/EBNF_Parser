using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace EBNF_Parser.Core
{
    public interface IElement
    {
        string ToString();

        bool TryParse(ReadOnlySpan<char> input, int start, Parser parser, [MaybeNullWhen(false)] out Parsed parsed);

        internal static string IdentifierPattern { get; } = "[a-zA-Z_][a-zA-Z0-9\\s_]*?";
        private static string GrouppingPattern { get; } = @"^\s*{0}\s*(.*?)\s*{1}\s*$";

        protected static bool TryParseGroupping(string input, string groupOpen, string groupClose, [MaybeNullWhen(false)] out IElement element, Func<string, IElement> ctor)
        {
            var match = Regex.Match(input, string.Format(GrouppingPattern, groupOpen, groupClose));

            element = default;
            return match is { Success: true, Groups: { Count: >= 2 } g }
                && g[1].Value is var elem
                && ((element = ctor(elem)) is not null);
        }

        public static bool TryParse(string input, [MaybeNullWhen(false)] out IElement element)
            => Quantifier.TryParse(input, out element)
            // || Exception.TryParse(input, out element)
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