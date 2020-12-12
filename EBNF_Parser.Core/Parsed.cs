namespace EBNF_Parser.Core
{
    public class Parsed
    {
        public Parsed(string value, IElement parser, int length, params Parsed[]? children)
        {
            Value = value;
            Parser = parser;
            Length = length;
            Children = children;
        }

        public string Value { get; }
        public IElement Parser { get; }
        public int Length { get; }
        public Parsed[]? Children { get; }

        public Parsed With(IElement element)
            => new(Value, element, Length, this);

        public override string ToString()
            => Value;
    }
}
