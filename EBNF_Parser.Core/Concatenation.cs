using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace EBNF_Parser.Core
{
    public class Concatenation : MultiElement
    {
        public Concatenation(IEnumerable<IElement> elements)
            : this (elements.ToArray())
        { }

        public Concatenation(params IElement[] elements)
            : base(elements)
        { }

        public override string ToString()
            => string.Join<IElement>(", ", Elements);

        public override bool TryParse(ReadOnlySpan<char> input, int start, Parser parser, [MaybeNullWhen(false)] out Parsed parsed)
        {
            var length = 0;
            parsed = default;
            var list = new List<Parsed>(Elements.Length);
            foreach (var element in Elements)
            {
                if (!element.TryParse(input[length..], length + start, parser, out var l))
                    return false;
                length += l.Length;
                list.Add(l);
            }
            parsed = new(input[..length].ToString(), this, start, length, list.ToArray());
            return true;
        }

        public static bool TryParse(string input, [MaybeNullWhen(false)] out Concatenation concatenation)
        {
            var isOk = TryParse(input, out IElement? element);
            concatenation = element as Concatenation;
            return isOk;
        }

        internal static bool TryParse(string input, [MaybeNullWhen(false)] out IElement element)
            => TryParse(input, ",", out element, (elem1, elem2) => new Concatenation(elem1, elem2).Simplify());

        private Concatenation Simplify()
        {
            if (Elements is {Length: 2} && Elements[1] is Concatenation c)
                return new(c.Elements.Prepend(Elements[0]));
            return this;
        }
    }
}