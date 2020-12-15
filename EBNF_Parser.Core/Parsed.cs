using System.Collections.Generic;
using System.Linq;

namespace EBNF_Parser.Core
{
    public class Parsed
    {
        public Parsed(string value, IElement parser, int start, int length, params Parsed[] children)
        {
            Value = value;
            Parser = parser;
            Start = start;
            Length = length;
            Parent = null;
            Children = children;
        }

        private Parsed(string value, IElement parser, int start, int length, Parsed? parent, params Parsed[] children)
        {
            Value = value;
            Parser = parser;
            Start = start;
            Length = length;
            Parent = parent;
            Children = children.Select(child => child.With(this)).ToArray();
        }

        public string Value { get; }
        public IElement Parser { get; }
        public int Start { get; }
        public int Length { get; }
        public Parsed? Parent { get; }
        public Parsed[] Children { get; }

        public Parsed With(IElement element)
            => new(Value, element, Start, Length, Parent, this);

        public Parsed With(Parsed parent)
            => new(Value, Parser, Start, Length, parent, Children);

        public override string ToString()
            => Value;
    }
}
