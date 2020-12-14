using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using EBNF_Parser.Core;

namespace EBNF_Parser.Parsers
{
    public static class JSON
    {
        private static readonly Parser _parser = Parser.ParseModel(File.ReadAllText("Tests Files\\json.ebnf"));

        public static bool TryParse(string json, [MaybeNullWhen(false)] out Token token)
        {
            token = default;
            if (!_parser.Rules["file"].TryParse(json, out var parsed))
                return false;
            token = Token.Create(parsed);
            return true;
        }
    }

    public abstract class Token
    {
        internal Token()
        { }

        internal static Token Create(Parsed parsed)
            => parsed.Children[0] switch
            {
                { Parser: Identifier { Value: "string" }, Children: var c } => new String(c[0].Children[1]),
                { Parser: Identifier { Value: "object" }, Children: var c } => new Object(c[0].Children[1]),
                { Parser: Identifier { Value: "array"  }, Children: var c } => new Array (c[0].Children[1]),
            };
    }

    public sealed class String : Token
    {
        internal String(Parsed parsed)
        {
            Value = parsed.Value;
        }

        public string Value { get; }
    }

    public class Object : Token
    {
        internal Object(Parsed properties)
        {
            Properties = GetProperties().ToArray();

            IEnumerable<Property> GetProperties()
            {
                yield return new Property(properties.Children[0].Children[1]);
                foreach (var property in properties.Children[0].Children[2].Children)
                    yield return new Property(property.Children[1]);
            }
        }

        public IEnumerable<Property> Properties { get; }
    }

    public class Array : Token
    {
        internal Array(Parsed properties)
        {
            Values = GetValues().ToArray();

            IEnumerable<Token> GetValues()
            {
                yield return Token.Create(properties.Children[0].Children[1]);
                foreach (var property in properties.Children[0].Children[2].Children)
                    yield return Token.Create(property.Children[1]);
            }
        }

        public IEnumerable<Token> Values { get; }
    }

    public class Property
    {
        internal Property(Parsed property)
        {
            Key = property.Children[0].Children[0].Children[0].Children[1].Value;
            Value = Token.Create(property.Children[0].Children[2]);
        }

        public string Key { get; }
        public Token Value { get; }
    }
}