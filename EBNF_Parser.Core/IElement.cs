namespace EBNF_Parser.Core
{
    public interface IElement
    {
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
        public Alternation(IElement left, IElement right)
        {
            Left = left;
            Right = right;
        }

        public IElement Left { get; }
        public IElement Right { get; }
    }

    public class Concatenation : IElement
    {
        public Concatenation(IElement left, IElement right)
        {
            Left = left;
            Right = right;
        }

        public IElement Left { get; }
        public IElement Right { get; }
    }

    public class Option : IElement
    {
        public Option(IElement value)
        {
            Value = value;
        }

        public IElement Value { get; }
    }

    public class Repetition : IElement
    {
        public Repetition(IElement value)
        {
            Value = value;
        }

        public IElement Value { get; }
    }

    public class Group : IElement
    {
        public Group(IElement value)
        {
            Value = value;
        }

        public IElement Value { get; }
    }

    public class String : IElement
    {
        public String(string value)
        {
            Value = value;
        }

        public string Value { get; }
    }

    public class Comment : IElement
    {
        public Comment(string value)
        {
            Value = value;
        }

        public string Value { get; }
    }

    public class Special : IElement
    {
        public Special(string value)
        {
            Value = value;
        }

        public string Value { get; }
    }

    public class Exception : IElement
    {
        public Exception(string value)
        {
            Value = value;
        }

        public string Value { get; }
    }
}