using System.IO;
using EBNF_Parser.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EBNF_Parser.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var parser = Parser.ParseModel(@"fr = ( ""bonjour"", "" "", ""ça va "" );
en = ""hi"", "" how are you "";
hi = (fr | en), { 2 * ""?"" } | ? binary 65 ?;");
            parser.Specials["binary"] = static (string input, Special element, out Parsed p) => byte.TryParse(element.Parameter, out var b) && (byte)input[0] == b ? (p = new(input[..1], element, 1)) is not null : (p = default!) is not null;
            var parsed = parser.Rules["hi"].TryParse("hi how are you ????", out var p);
            parsed = parser.Rules["hi"].TryParse("hi how are you ?????", out p);
            parsed = parser.Rules["hi"].TryParse("hi how are you ??????", out p);
            parsed = parser.Rules["hi"].TryParse("A", out p);

            Assert.IsTrue(IElement.TryParse(@"""|"" | "",""", out var elem));
            Assert.IsTrue(elem is Alternation { Elements: { Length: 2 } e } && e[0] is String { Value: "|" } && e[1] is String { Value: "," });
            Assert.IsTrue(Parser.ParseModel("plop = \"gfcvn,b\\\"\\\\'\";") is {Rules: { Count: 1 } r } && r["plop"] is { Identifier: "plop", Element: String { Value: "gfcvn,b\"'" }});
            Parser.ParseModel("plap = \"gfcvn,b\\\"\\\\'\"");
            Parser.ParseModel("plip = 'gfcvn,b\"\\\\\\'';");
            Assert.ThrowsException<InvalidRuleDefinitionSyntax>(() => Parser.ParseModel("id = 'id' | ;"));
            Assert.ThrowsException<InvalidRuleSyntax>(() => Parser.ParseModel("id id;"));
            Assert.ThrowsException<CyclicReferenceException>(() => Parser.ParseModel("id = id;"));
            Assert.ThrowsException<UnreferencedIdentifierException>(() => Parser.ParseModel("id = _id0;"));
            parsed = Parser.TryParseFile("Tests Files\\hello.bf", "program", out p);
            parser = Parser.ParseModel(File.ReadAllText("Tests Files\\bf.ebnf"));
            parsed = parser.Rules["program"].TryParse(File.ReadAllText("Tests Files\\hello.bf"), out p);
        }
    }
}
