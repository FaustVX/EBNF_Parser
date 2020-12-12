using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace EBNF_Parser.Core
{
    public abstract class SingleElement : IElement
    {
        private static string GrouppingPattern { get; } = @"^\s*{0}\s*(.*?)\s*{1}\s*$";

        public SingleElement(IElement element)
        {
            Element = element;
        }

        public IElement Element { get; }

        public abstract bool TryParse(string input, Parser parser, [MaybeNullWhen(false)] out Parsed parsed);

        public abstract override string ToString();

        protected static bool TryParse(string input, string groupOpen, string groupClose, [MaybeNullWhen(false)] out IElement element, Func<IElement, IElement> ctor)
        {
            var match = Regex.Match(input, string.Format(GrouppingPattern, groupOpen, groupClose));

            element = default;
            return match is { Success: true, Groups: { Count: >= 2 } g }
                && IElement.TryParse(g[1].Value, out var elem)
                && ((element = ctor(elem)) is not null);
        }
    }
}