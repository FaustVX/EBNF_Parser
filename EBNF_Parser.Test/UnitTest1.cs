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
            IElement.TryParse(@"""|"" | "",""", out var elem);
            Parser.ParseModel("plop = \"gfcvn,b\\\"\\\\'\";");
            Parser.ParseModel("plap = \"gfcvn,b\\\"\\\\'\"");
            Parser.ParseModel("plip = 'gfcvn,b\"\\\\\\'';");
            Assert.ThrowsException<System.Exception>(() => Parser.ParseModel("plup = 'gfcvn,b\\\"\\\\'';"));
            Parser.ParseModel(File.ReadAllText("Pascal.ebnf"));
            Parser.ParseModel(File.ReadAllText("EBNF.ebnf"));
        }
    }
}
