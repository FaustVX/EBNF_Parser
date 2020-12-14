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
            Children = children;
        }

        public string Value { get; }
        public IElement Parser { get; }
        public int Start { get; }
        public int Length { get; }
        public Parsed[] Children { get; }

        public Parsed With(IElement element)
            => new(Value, element, Start, Length, this);

        public override string ToString()
            => Value;
    }
}
