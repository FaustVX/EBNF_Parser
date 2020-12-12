namespace EBNF_Parser.Core
{
    public class Parsed
    {
        public Parsed(string value, IElement parser)
        {
            Value = value;
            Parser = parser;
        }

        public string Value { get; }
        public IElement Parser { get; }
    }
}
