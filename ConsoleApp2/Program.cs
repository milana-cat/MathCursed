using System;
using System.Collections.Generic;
using System.Linq;

public class GrammarRule
{
    public char NonTerminal { get; set; }
    public List<string> Productions { get; set; }

    public GrammarRule(char nonTerminal, List<string> productions) // Hello from Avazbek!
    {
        NonTerminal = nonTerminal;
        Productions = productions;
    }
}

public class MainClass
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

    public static List<GrammarRule> SplitLinesForGrammarRule(List<string> lines)
    {
        bool IsCorrectLine(string line)
        {
            return line.Length > 2 && (char.IsLetter(line[0]) && char.IsUpper(line[0]) || char.IsDigit(line[0]))
                                   && line[1] == '-' && line[2] == '>';
        }

        List<GrammarRule> grammarRules = new List<GrammarRule>();

        for (int i = 0; i < lines.Count; i++)
        {
            if (IsCorrectLine(lines[i]))
            {
                if (grammarRules.Any(rules => rules.NonTerminal == lines[i][0]) == false) // if character not existing
                    grammarRules.Add(new GrammarRule(lines[i][0],
                        lines[i].Remove(0, 3).Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries).ToList()));
                else
                {
                    int index = grammarRules.FindIndex(rules => rules.NonTerminal == lines[i][0]);
                    grammarRules[index].Productions.AddRange(lines[i].Remove(0, 3)
                        .Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries).ToList());
                }
            }
        }

        return grammarRules;
    }

    public static void Main(string[] args)
    {
        // Пример входной КС-грамматики с правой рекурсией
        // var grammarRules = new List<GrammarRule>
        // {
        //     new GrammarRule('S', new List<string> { "SaA", "SbA","AA" }),
        //     new GrammarRule('A', new List<string> { "AdB", "AcB","BB" }),
        //     new GrammarRule('B', new List<string> { "SSc", "g", "f" })
        // };
        //
        // var newGrammarRules = RemoveRightRecursion(grammarRules);
        //
        // foreach (var rule in newGrammarRules)
        // {
        //     Console.WriteLine(rule.NonTerminal + " -> " + string.Join(" | ", rule.Productions));
        //     
        // }
        // Console.ReadKey();
        var testList = new List<string>();
        testList.Add("S->SaA|SbA|AA|AD");
        testList.Add("A->AdB|AcB|BB");
        testList.Add("B->SSc|g|f");
        testList.Add("S->SaA|SbA|AA|AD");
        testList.Add("1->AdB|AcB|BB");

        // Incorrect data
        testList.Add("AS->SaD");
        testList.Add("1->");
        testList.Add("a-z->");
        testList.Add("");
        testList.Add("->");
        testList.Add("G->||,|");

        var grammarRules = SplitLinesForGrammarRule(testList);

        foreach (var rule in grammarRules)
        {
            Console.WriteLine(string.Join("|", rule.Productions));
        }
    }
}