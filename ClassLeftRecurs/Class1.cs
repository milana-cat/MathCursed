using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLeftRecurs
{
    public class GrammarRule
    {
        public char NonTerminal { get; set; }
        public List<string> Productions { get; set; }

        public GrammarRule(char nonTerminal, List<string> productions) 
        {
            NonTerminal = nonTerminal;
            Productions = productions;
        }
    }
    public class GrammarOperation
    {
        public static  bool IsCorrectLine(string line)
        {
            return line.Length > 2 && (char.IsLetter(line[0]) && char.IsUpper(line[0]) || char.IsDigit(line[0]))
                                   && line[1] == '-' && line[2] == '>';
        }
        public static Dictionary<string, List<string>> Converter(List<GrammarRule> grammarRule)
        {
            Dictionary<string, List<string>> resultGrammar = new Dictionary<string, List<string>>();
            foreach (var rule in grammarRule)
            {
                resultGrammar.Add(rule.NonTerminal.ToString(), rule.Productions);
            }
            return resultGrammar;
        }
        public static List<GrammarRule> SplitLinesForGrammarRule(List<string> lines)
        {
            

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
                else
                {
                    throw new Exception();
                }
            }

            return grammarRules;
        }

        public static void IndirectLeftRecursion(Dictionary<string, List<string>> rules)
        {
            int count = rules.Keys.Count();
            for (int i = 0; i < count; i++)
            {
                string nonterminal = rules.Keys.ElementAt(i);
                var reachableNonterminals = new HashSet<string>();
                //FindReachableNonterminals(rules, nonterminal, reachableNonterminals);

                if (i >= 1)
                {
                    for (int j = 0; j < i; j++)
                    {
                        string nonterminalsecond = rules.Keys.ElementAt(j);
                        DeepRecursionSubstring(rules, nonterminal, nonterminalsecond);
                        if (j == i - 1)
                        {
                            SimpleRecursionSubstring(rules, nonterminal);
                        }
                    }
                }
                else
                {
                    SimpleRecursionSubstring(rules, nonterminal);
                }


            }
        }
        public static void SimpleRecursionSubstring(Dictionary<string, List<string>> rules, string nonterminal)
        {
            string newNonterminal = nonterminal.ToString() + "'";
            var newRules = new List<string>();
            var newRulesNonTerm = new List<string>();
            bool recurrs = false;
            for (int j = 0; j < rules[nonterminal].Count; j++)
            {
                string rule = rules[nonterminal][j];
                if (rule.StartsWith(nonterminal))
                {
                    newRulesNonTerm.Add(rule.Substring(1) + newNonterminal);
                    newRulesNonTerm.Add(rule.Substring(1));
                    recurrs = true;
                }
                else
                {
                    newRules.Add(rule);
                    if (recurrs)
                        newRules.Add(rule + newNonterminal);
                }
            }
            rules[nonterminal] = newRules;
            if (recurrs)
                rules[newNonterminal] = newRulesNonTerm;
        }
        public static void DeepRecursionSubstring(Dictionary<string, List<string>> rules, string nonterminal1, string nonterminal2)
        {

            List<string> newRules2 = new List<string>();


            for (int i = 0; i < rules[nonterminal1].Count; i++)
            {
                string rule2 = rules[nonterminal1][i];
                if (rule2.StartsWith(nonterminal2))
                {
                    for (int j = 0; j < rules[nonterminal2].Count; j++)
                    {
                        string s = rules[nonterminal2][j];
                        newRules2.Add(s + rule2.Substring(nonterminal2.Length));

                    }
                    //i += rules[nonterminal2].Count;
                }
                else
                {
                    newRules2.Add(rule2);
                }

            }
            rules[nonterminal1] = newRules2;
            //SimpleRecursionSubstring(rules, nonterminal1);
        }

        public static Dictionary<string, List<string>> GetGrammarRules(List<string> grammar)
        {
            Dictionary<string, List<string>> newGrammar = new Dictionary<string, List<string>>();
            newGrammar = Converter(SplitLinesForGrammarRule(grammar));
            IndirectLeftRecursion(newGrammar);
            return newGrammar;
        }




    }
}
