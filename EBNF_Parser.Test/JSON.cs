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
                { Parser: Identifier { Value: "string" } } => new String(parsed.FindFirst("characters")!),
                { Parser: Identifier { Value: "object" } } => new Object(parsed.FindFirst("properties")!),
                { Parser: Identifier { Value: "array"  } } => new Array (parsed.FindFirst("tokens")!),
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
                var property = properties.FindFirst("property");
                if (property is null)
                    yield break;
                yield return new Property(property);
                for (property = property.Next?.FindFirst("property"); property is not null; property = property.Parent?.Next?.FindFirst("property"))
                    yield return new Property(property);
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
                var property = properties.FindFirst("token");
                if (property is null)
                    yield break;
                yield return Token.Create(property);
                for (property = property.Next?.FindFirst("token"); property is not null; property = property.Parent?.Next?.FindFirst("token"))
                    yield return Token.Create(property);
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