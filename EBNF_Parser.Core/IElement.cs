using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.RegularExpressions;

namespace EBNF_Parser.Core
{
    public interface IElement
    {
        string ToString();

        internal static bool TryParse(string input, [MaybeNullWhen(false)] out IElement element)
            => Alternation.TryParse(input, out element)
            || Concatenation.TryParse(input, out element)
            || Option.TryParse(input, out element)
            || Repetition.TryParse(input, out element)
            || Group.TryParse(input, out element)
            || String.TryParse(input, out element)
            || Comment.TryParse(input, out element)
            || Special.TryParse(input, out element)
            || Exception.TryParse(input, out element)
            || false;
    }

    public class Rule
    {
        public Rule (string identifier, IElement element)
        {
            Identifier = identifier;
            Element = element;
        }

        public string Identifier { get; }
        public IElement Element { get; }

        public static bool TryParse(string input, [MaybeNullWhen(false)] out Rule rule)
        {
            var match = Regex.Match(input, @"\s*(.*?)\s*=\s*(.*)");

            rule = default;
            return match is { Success: true, Groups: { Count: >= 3 } g }
                && IElement.TryParse(g[2].Value, out var elem)
                && ((rule = new Rule(g[1].Value, elem)) is not null);
        }
    }

    public class Alternation : IElement
    {
        public Alternation(IEnumerable<IElement> elements)
            : this(elements.ToArray())
        { }

        public Alternation(params IElement[] elements)
        {
            Elements = elements;
        }

        public IElement[] Elements { get; }

        public override string ToString()
            => string.Join<IElement>(" | ", Elements);

        public static bool TryParse(string input, [MaybeNullWhen(false)] out Alternation alternation)
        {
            var isOk = TryParse(input, out IElement? element);
            alternation = element as Alternation;
            return isOk;
        }

        internal static bool TryParse(string input, [MaybeNullWhen(false)] out IElement element)
        {
            var match = Regex.Match(input, @"\s*(.*?)\s*\|\s*(.*)\s*");

            element = default;
            return match is { Success: true, Groups: { Count: >= 3 } g }
                && IElement.TryParse(g[1].Value, out var elem1)
                && IElement.TryParse(g[2].Value, out var elem2)
                && ((element = new Alternation(elem1, elem2).Simplify()) is not null);
        }

        private Alternation Simplify()
        {
            if (Elements is {Length: 2} && Elements[1] is Alternation c)
                return new(c.Simplify().Elements.Prepend(Elements[0]));
            return this;
        }
    }

    public class Concatenation : IElement
    {
        public Concatenation(IEnumerable<IElement> elements)
            : this (elements.ToArray())
        { }

        public Concatenation(params IElement[] elements)
        {
            Elements = elements;
        }

        public IElement[] Elements { get; }

        public override string ToString()
            => string.Join<IElement>(", ", Elements);

        public static bool TryParse(string input, [MaybeNullWhen(false)] out Concatenation concatenation)
        {
            var isOk = TryParse(input, out IElement? element);
            concatenation = element as Concatenation;
            return isOk;
        }

        internal static bool TryParse(string input, [MaybeNullWhen(false)] out IElement element)
        {
            var match = Regex.Match(input, @"\s*(.*?)\s*,\s*(.*)\s*");

            element = default;
            return match is { Success: true, Groups: { Count: >= 3 } g }
                && IElement.TryParse(g[1].Value, out var elem1)
                && IElement.TryParse(g[2].Value, out var elem2)
                && ((element = new Concatenation(elem1, elem2).Simplify()) is not null);
        }

        private Concatenation Simplify()
        {
            if (Elements is {Length: 2} && Elements[1] is Concatenation c)
                return new(c.Simplify().Elements.Prepend(Elements[0]));
            return this;
        }
    }

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
            var match = Regex.Match(input, @"\s*\[\s*(.*?)\s*\]\s*");

            element = default;
            return match is { Success: true, Groups: { Count: >= 2 } g }
                && IElement.TryParse(g[1].Value, out var elem)
                && ((element = new Option(elem)) is not null);
        }
    }

    public class Repetition : IElement
    {
        public Repetition(IElement value)
        {
            Value = value;
        }

        public IElement Value { get; }

        public override string ToString()
            => $"{{ {Value} }}";

        public static bool TryParse(string input, [MaybeNullWhen(false)] out Repetition repetition)
        {
            var isOk = TryParse(input, out IElement? element);
            repetition = element as Repetition;
            return isOk;
        }

        internal static bool TryParse(string input, [MaybeNullWhen(false)] out IElement element)
        {
            var match = Regex.Match(input, @"\s*{\s*(.*?)\s*}\s*");

            element = default;
            return match is { Success: true, Groups: { Count: >= 2 } g }
                && IElement.TryParse(g[1].Value, out var elem)
                && ((element = new Repetition(elem)) is not null);
        }
    }

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
        {
            var match = Regex.Match(input, @"\s*\(\s*(.*?)\s*\)\s*");

            element = default;
            return match is { Success: true, Groups: { Count: >= 2 } g }
                && IElement.TryParse(g[1].Value, out var elem)
                && ((element = new Group(elem)) is not null);
        }
    }

    public class String : IElement
    {
        public String(string value)
        {
            Value = value;
        }

        public string Value { get; }

        public override string ToString()
            => $"\" {Value} \"";

        public static bool TryParse(string input, [MaybeNullWhen(false)] out String @string)
        {
            var isOk = TryParse(input, out IElement? element);
            @string = element as String;
            return isOk;
        }

        internal static bool TryParse(string input, [MaybeNullWhen(false)] out IElement element)
        {
            var match = Regex.Match(input, "\\s*\"(.*?)\"\\s*");

            element = default;
            return match is { Success: true, Groups: { Count: >= 2 } g }
                && ((element = new String(g[1].Value)) is not null);
        }
    }

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
            var match = Regex.Match(input, Regex.Escape(@"\s*\(\*(.*?)\*\)\s*"));

            element = default;
            return match is { Success: true, Groups: { Count: >= 2 } g }
                && ((element = new Comment(g[1].Value)) is not null);
        }
    }

    public class Special : IElement
    {
        public Special(string value)
        {
            Value = value;
        }

        public string Value { get; }

        public override string ToString()
            => $"? {Value} ?";

        public static bool TryParse(string input, [MaybeNullWhen(false)] out Special special)
        {
            var isOk = TryParse(input, out IElement? element);
            special = element as Special;
            return isOk;
        }

        internal static bool TryParse(string input, [MaybeNullWhen(false)] out IElement element)
        {
            var match = Regex.Match(input, @"\s*?(.*?)?\s*");

            element = default;
            return match is { Success: true, Groups: { Count: >= 2 } g }
                && ((element = new Special(g[1].Value)) is not null);
        }
    }

    public class Exception : IElement
    {
        public Exception(string value)
        {
            Value = value;
        }

        public string Value { get; }

        public override string ToString()
            => $"- {Value}";

        public static bool TryParse(string input, [MaybeNullWhen(false)] out Exception exception)
        {
            var isOk = TryParse(input, out IElement? element);
            exception = element as Exception;
            return isOk;
        }

        internal static bool TryParse(string input, [MaybeNullWhen(false)] out IElement element)
        {
            var match = Regex.Match(input, @"\s*-\s*(.*)");

            element = default;
            return match is { Success: true, Groups: { Count: >= 2 } g }
                && IElement.TryParse(g[0].Value, out element);
        }
    }
}