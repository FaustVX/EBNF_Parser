using System.Collections.Generic;

namespace EBNF_Parser.Core
{
    public interface IElement
    {
        string ToString();
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
    }

    public class Alternation : IElement
    {
        public Alternation(IEnumerable<IElement> elements)
        {
            Elements = elements;
        }

        public IEnumerable<IElement> Elements { get; }

        public override string ToString()
            => string.Join(" | ", Elements);
    }

    public class Concatenation : IElement
    {
        public Concatenation(IEnumerable<IElement> elements)
        {
            Elements = elements;
        }

        public IEnumerable<IElement> Elements { get; }

        public override string ToString()
            => string.Join(", ", Elements);
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
    }
}