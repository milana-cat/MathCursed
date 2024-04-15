using System;
using System.Collections.Generic;

class GrammarRule
{
    public char NonTerminal { get; set; }
    public List<string> Productions { get; set; }

    public GrammarRule(char nonTerminal, List<string> productions)
    {
        NonTerminal = nonTerminal;
        Productions = productions;
    }
}

class MainClass
{
    static List<GrammarRule> RemoveRightRecursion(List<GrammarRule> grammarRules)
    {
        List<GrammarRule> newGrammarRules = new List<GrammarRule>();

        foreach (var rule in grammarRules)
        {
            var currentNonTerminal = rule.NonTerminal;
            var currentProductions = rule.Productions;

            List<string> newProductions = new List<string>();
            List<string> recursiveProductions = new List<string>();

            foreach (var production in currentProductions)
            {
                if (production[0] == currentNonTerminal)
                {
                    recursiveProductions.Add(production.Substring(1) + currentNonTerminal + "'");
                }
                else
                {
                    newProductions.Add(production + currentNonTerminal + "'");
                }
            }

            if (recursiveProductions.Count > 0)
            {
                newProductions.Add("");
                newProductions.Add(currentNonTerminal + "' -> " + string.Join(" | ", recursiveProductions) + " | eps");
            }

            newGrammarRules.Add(new GrammarRule(currentNonTerminal, newProductions));
        }

        return newGrammarRules;
    }

    public static void Main(string[] args)
    {
        // Пример входной КС-грамматики с правой рекурсией
        var grammarRules = new List<GrammarRule>
        {
            new GrammarRule('S', new List<string> { "SaA", "SbA","AA" }),
            new GrammarRule('A', new List<string> { "AdB", "AcB","BB" }),
            new GrammarRule('B', new List<string> { "SSc", "g", "f" })
        };

        var newGrammarRules = RemoveRightRecursion(grammarRules);

        foreach (var rule in newGrammarRules)
        {
            Console.WriteLine(rule.NonTerminal + " -> " + string.Join(" | ", rule.Productions));
            
        }
        Console.ReadKey();
    }
}
