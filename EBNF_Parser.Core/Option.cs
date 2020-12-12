using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace EBNF_Parser.Core
{
    public class Option : IElement
    {
        public Option(IElement value)
        {
            Value = value;
        }

        public IElement Value { get; }

        public override string ToString()
            => $"[ {Value} ]";

        public static bool TryParse(string input, [MaybeNullWhen(false)] out Option option)
        {
            var isOk = TryParse(input, out IElement? element);
            option = element as Option;
            return isOk;
        }

        internal static bool TryParse(string input, [MaybeNullWhen(false)] out IElement element)
        {
            var match = Regex.Match(input, @"^\s*\[\s*(.*?)\s*\]\s*$");

            element = default;
            return match is { Success: true, Groups: { Count: >= 2 } g }
                && IElement.TryParse(g[1].Value, out var elem)
                && ((element = new Option(elem)) is not null);
        }
    }
}