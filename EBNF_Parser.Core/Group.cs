using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace EBNF_Parser.Core
{
    public class Group : SingleElement
    {
        public Group(IElement value)
            : base(value)
        { }

        public override string ToString()
            => $"( {Element} )";

        public override bool TryParse(ReadOnlySpan<char> input, int start, Parser parser, [MaybeNullWhen(false)] out Parsed parsed)
            => Element.TryParse(input, start, parser, out parsed);

        public static bool TryParse(string input, [MaybeNullWhen(false)] out Group group)
        {
            var isOk = TryParse(input, out IElement? element);
            group = element as Group;
            return isOk;
        }

        internal static bool TryParse(string input, [MaybeNullWhen(false)] out IElement element)
            => TryParse(input, @"\(", @"\)", out element, elem => new Group(elem));
    }
}