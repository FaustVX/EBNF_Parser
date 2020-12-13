using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace EBNF_Parser.Core
{
    public class UnreferencedIdentifierException : System.Exception
    {
        public UnreferencedIdentifierException(string identifier)
        {
            Identifier = identifier;
        }

        public string Identifier { get; }
    }

    public class CyclicReferenceException : System.Exception
    {
        public CyclicReferenceException(string identifier)
        {
            Identifier = identifier;
        }

        public string Identifier { get; }
    }

    public class InvalidRuleSyntax : System.Exception
    {
        public InvalidRuleSyntax(string triedRule)
        {
            TriedRule = triedRule;
        }

        public string TriedRule { get; }
    }

    public class InvalidRuleDefinitionSyntax : System.Exception
    {
        public InvalidRuleDefinitionSyntax(string ruleId, string definition)
        {
            RuleId = ruleId;
            Definition = definition;
        }

        public string RuleId { get; }
        public string Definition { get; }
    }
}
