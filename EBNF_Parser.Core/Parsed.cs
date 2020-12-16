using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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

        private Parsed(string value, IElement parser, int start, int length, int idx, Parsed? parent, params Parsed[] children)
        {
            Value = value;
            Parser = parser;
            Start = start;
            Length = length;
            Parent = parent;
            Children = children.Select((child, idx) => child.With(this, idx)).ToArray();
            Index = idx;
        }

        public string Value { get; }
        public IElement Parser { get; }
        public int Start { get; }
        public int Length { get; }
        public Parsed? Parent { get; }
        public Parsed[] Children { get; }
        public Parsed Root => ((Parent is not null) ? Parent.Root : Parent) ?? this;
        public Parsed? Previous => Index >= 1 && Parent?.Children is { Length: >= 2 } c0 ? c0[Index - 1] : null;
        public Parsed? Next => Parent?.Children is { Length: >= 2 and var l } c1 && l > Index + 1 ? c1[Index + 1] : null;
        public int Index { get; }

        public Parsed? FindFirst(string identifier)
        {
            return SelectMany(this).FirstOrDefault(p => p.Parser is Identifier id && id.Value == identifier);

            static IEnumerable<Parsed> SelectMany(Parsed parsed)
                => parsed.Children.SelectMany(SelectMany).Prepend(parsed);
        }

        public bool Modify(string value, Parser parser, [MaybeNullWhen(false)] out Parsed parsed)
            => Root.Parser.TryParse(Root.Value[..Start] + value + Root.Value[(Start + Length)..], Start, parser, out parsed) && (parsed = parsed!.With(default(Parsed)!, 0)) is not null;

        public Parsed With(IElement element)
            => new(Value, element, Start, Length, this);

        public Parsed With(Parsed parent, int idx)
            => new(Value, Parser, Start, Length, idx, parent, Children);

        public override string ToString()
            => Value;
    }
}
