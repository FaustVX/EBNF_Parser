using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace EBNF_Parser.Core
{
    public class Group : IElement
    {
        public Group(IElement value)
        {
            Value = value;
        }

        public IElement Value { get; }

        public override string ToString()
            => $"( {Value} )";

        public static bool TryParse(string input, [MaybeNullWhen(false)] out Group group)
        {
            var isOk = TryParse(input, out IElement? element);
            group = element as Group;
            return isOk;
        }

        internal static bool TryParse(string input, [MaybeNullWhen(false)] out IElement element)
            => IElement.TryParseGroupping(input, @"\(", @"\)", out element, elem => new Group(elem));
    }
}