using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace EBNF_Parser.Core
{
    public class String : IElement
    {
        public String(string value)
        {
            Value = value;
        }

        public string Value { get; }

        public override string ToString()
            => $"\"{Value}\"";

        public bool TryParse(string input, Parser parser, [MaybeNullWhen(false)] out int length)
        {
            length = 0;
            if (input.StartsWith(Value))
                length = Value.Length;
            else
                return false;
            return true;
        }

        public static bool TryParse(string input, [MaybeNullWhen(false)] out String @string)
        {
            var isOk = TryParse(input, out IElement? element);
            @string = element as String;
            return isOk;
        }

        internal static bool TryParse(string input, [MaybeNullWhen(false)] out IElement element)
        {
            // var doubleQuote = $"^(?:\\s*\"((?:{IElement.CharPattern + "|\\\\\\\"|\\\\\\\\|'"})*?)\"\\s*)$";
            // var singleQuote = $"^(?:\\s*'((?:{IElement.CharPattern + "|\\\\\\'|\\\\\\\\|\""})*?)'\\s*)$";
            // var pattern = $"{doubleQuote}|{singleQuote}"; // to print: " , in file: \" , in Regex: \\\" , in String: \\\\\\\"
            var pattern = @"^(?:\s*'((?:[^""\\]|\\'|\\\\|\""|\\r|\\n)+?)'\s*)$|^(?:\s*""((?:[^""\\]|\\\""|\\\\|'|\\r|\\n)+?)""\s*)$";
            var match = Regex.Match(input, pattern);

            element = default;
            return match is { Success: true, Groups: { Count: >= 3 } g }
                && ((g[1] is { Success: true, Value: var val1 } && (element = new String(val1)) is not null)
                    || (g[2] is { Success: true, Value: var val2 } && (element = new String(val2)) is not null));
        }
    }
}