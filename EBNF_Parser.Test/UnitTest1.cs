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
            var parser = Parser.ParseModel(@"fr = ( ""bonjour"", "" "", ""ça va "" ), { ""?"" };
en = ""hi"", "" how are you "", { ""?"" };
hi = fr | en");
            var parsed = parser.Rules["hi"].TryParse("hi how are you ????", out var p);

            Assert.IsTrue(IElement.TryParse(@"""|"" | "",""", out var elem));
            Assert.IsTrue(elem is Alternation { Elements: { Length: 2 } e } && e[0] is String { Value: "|" } && e[1] is String { Value: "," });
            // Assert.IsTrue(Parser.ParseModel("plop = \"gfcvn,b\\\"\\\\'\";") is {Rules: { Length: 1 } r } && r[0] is { Identifier: "plop", Element: String { Value: "gfcvn,b\\\"\\\\'" }});
            Parser.ParseModel("plap = \"gfcvn,b\\\"\\\\'\"");
            Parser.ParseModel("plip = 'gfcvn,b\"\\\\\\'';");
            Assert.ThrowsException<System.Exception>(() => Parser.ParseModel("plup = 'gfcvn,b\\\"\\\\'';"));
            parser = Parser.ParseModel(File.ReadAllText("Tests Files\\bf.ebnf"));
            parsed = parser.Rules["program"].TryParse(File.ReadAllText("Tests Files\\hello.bf"), out p);
            parser = Parser.ParseModel(File.ReadAllText("Tests Files\\Pascal.ebnf"));
            parsed = parser.Rules["program"].TryParse(File.ReadAllText("Tests Files\\DEMO1.pascal"), out p);
            parser = Parser.ParseModel(File.ReadAllText("Tests Files\\EBNF.ebnf"));
            parsed = parser.Rules["grammar"].TryParse(File.ReadAllText("Tests Files\\EBNF.ebnf"), out p);
        }
    }
}
