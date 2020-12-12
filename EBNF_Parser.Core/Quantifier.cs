using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace EBNF_Parser.Core
{
    public class Quantifier : IElement
    {
        public Quantifier(int quantity, IElement element)
        {
            Quantity = quantity;
            Element = element;
        }

        public int Quantity { get; }
        public IElement Element { get; }

        public override string ToString()
            => $"{Quantity} * {Element}";

        public static bool TryParse(string input, [MaybeNullWhen(false)] out Exception exception)
        {
            var isOk = TryParse(input, out IElement? element);
            exception = element as Exception;
            return isOk;
        }

        internal static bool TryParse(string input, [MaybeNullWhen(false)] out IElement element)
        {
            var match = Regex.Match(input, @"^\s*(\d+)\s*\*\s*(.*)\s*$");

            element = default;
            return match is { Success: true, Groups: { Count: >= 3 } g }
                && int.TryParse(g[1].Value, out var qty)
                && IElement.TryParse(g[2].Value, out var elem)
                && ((element = new Quantifier(qty, elem)) is not null);
        }
    }
}