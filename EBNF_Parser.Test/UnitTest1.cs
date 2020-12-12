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
            Assert.IsTrue(IElement.TryParse(@"""|"" | "",""", out var elem));
            Assert.IsTrue(elem is Alternation { Elements: { Length: 2 } e } && e[0] is String { Value: "|" } && e[1] is String { Value: "," });
            // Assert.IsTrue(Parser.ParseModel("plop = \"gfcvn,b\\\"\\\\'\";") is {Rules: { Length: 1 } r } && r[0] is { Identifier: "plop", Element: String { Value: "gfcvn,b\\\"\\\\'" }});
            Parser.ParseModel("plap = \"gfcvn,b\\\"\\\\'\"");
            Parser.ParseModel("plip = 'gfcvn,b\"\\\\\\'';");
            Assert.ThrowsException<System.Exception>(() => Parser.ParseModel("plup = 'gfcvn,b\\\"\\\\'';"));
            Parser.ParseModel(File.ReadAllText("Pascal.ebnf"));
            Parser.ParseModel(File.ReadAllText("EBNF.ebnf"));
        }
    }
}
