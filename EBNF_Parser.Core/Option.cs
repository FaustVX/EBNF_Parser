using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace EBNF_Parser.Core
{
    public class Option : SingleElement
    {
        public Option(IElement value)
            : base(value)
        { }

        public override string ToString()
            => $"[ {Element} ]";

        public override bool TryParse(ReadOnlySpan<char> input, int start, Parser parser, [MaybeNullWhen(false)] out Parsed parsed)
            => (Element.TryParse(input, start, parser, out var p) && (parsed = p.With(this)) is not null) || (parsed = new Parsed("", this, start, 0)) is not null;

        public static bool TryParse(string input, [MaybeNullWhen(false)] out Option option)
        {
            var isOk = TryParse(input, out IElement? element);
            option = element as Option;
            return isOk;
        }

        internal static bool TryParse(string input, [MaybeNullWhen(false)] out IElement element)
            => TryParse(input, @"\[", @"\]", out element, elem => new Option(elem));
    }
}