using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace EBNF_Parser.Core
{
    public class Comment : IElement
    {
        public Comment(string value)
        {
            Value = value;
        }

        public string Value { get; }

        public override string ToString()
            => $"(* {Value} *)";

        public static bool TryParse(string input, [MaybeNullWhen(false)] out Comment comment)
        {
            var isOk = TryParse(input, out IElement? element);
            comment = element as Comment;
            return isOk;
        }

        internal static bool TryParse(string input, [MaybeNullWhen(false)] out IElement element)
        {
            var match = Regex.Match(input, Regex.Escape(@"^\s*\(\*(.*?)\*\)\s*$"));

            element = default;
            return match is { Success: true, Groups: { Count: >= 2 } g }
                && ((element = new Comment(g[1].Value)) is not null);
        }
    }
}