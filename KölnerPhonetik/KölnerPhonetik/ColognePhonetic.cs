using System.Collections.Generic;
using System.Linq;
using System.Text;


// http://gutmet.org/blog/2016/05/12/cologne-phonetics-slash-kolner-phonetik.html 

namespace KölnerPhonetik
{
    /// <summary>
    /// Implements the conversion of words to phonetic Codes by application of Cologne phonetics rules.
    /// </summary>
    /// <remarks>
    /// Cologne phonetics is supposed to yield better results than Soundex regarding German words.
    /// Contrary to Soundex, the length of the phonetic Code is not limited.
    /// </remarks>
    public class ColognePhonetic
    {
        /// <summary>
        /// Straight-forward translation of https://de.wikipedia.org/wiki/K%C3%B6lner_Phonetik to C#.
        /// </summary>
        internal class Rule
        {
            // Needed to express 'initial'
            public const char EmptyChar = '\0';

            public static Dictionary<char, Rule[]> All
            {
                get
                {
                    if (_rules == null)
                    {
                        _tempRules = new Dictionary<char, List<Rule>>();
                        _rules = new Dictionary<char, Rule[]>();
                        AddRule("AEIJOUYÖÜÄ", Code: "0");
                        AddRule("B", Code: "1");
                        AddRule("P", NotNext: "H", Code: "1");
                        AddRule("DT", NotNext: "CSZ", Code: "2");
                        AddRule("FVW", Code: "3");
                        AddRule("P", Next: "H", Code: "3");
                        AddRule("GKQ", Code: "4");
                        AddRule("C", Previous: EmptyChar + " ", Next: "AHKLOQRUX", Code: "4");
                        AddRule("C", Next: "AHKOQUX", NotPrevious: "SZ", Code: "4");
                        AddRule("X", NotPrevious: "CKQ", Code: "48");
                        AddRule("L", Code: "5");
                        AddRule("MN", Code: "6");
                        AddRule("R", Code: "7");
                        AddRule("SZß", Code: "8");
                        AddRule("C", Previous: "SZ", Code: "8");
                        AddRule("C", Previous: EmptyChar + " ", NotNext: "AHKLOQRUX", Code: "8");
                        AddRule("C", NotNext: "AHKOQUX", Code: "8");
                        AddRule("DT", Next: "CSZ", Code: "8");
                        AddRule("X", Previous: "CKQ", Code: "8");
                        FinalizeRules();
                    }
                    return _rules;
                }
            }
            private static Dictionary<char, List<Rule>> _tempRules;
            private static Dictionary<char, Rule[]> _rules;


            char letter;
            char[] NotPrevious;
            char[] NotNext;
            char[] Previous;
            char[] Next;
            public string Code { get; private set; }

            public Rule(char letter, string Code)
            {
                this.letter = letter;
                this.Code = Code;
            }

            private static bool Contains(char[] Arr, char c) => Arr == null || Arr.Contains(c);
            private static bool NotContains(char[] Arr, char c) => Arr == null || !Arr.Contains(c);

            public bool Applies(char prev, char curr, char next)
            {
                return curr == letter
                    && Contains(Previous, prev)
                    && NotContains(NotPrevious, prev)
                    && Contains(Next, next)
                    && NotContains(NotNext, next);
            }

            private static void AddRule(string Letters, string Code, string NotPrevious = null, string Previous = null, string NotNext = null, string Next = null)
            {
                char[] singleLetters = Letters.ToCharArray();

                foreach (var letter in singleLetters)
                {
                    if (!_tempRules.ContainsKey(letter))
                        _tempRules[letter] = new List<Rule>();

                    _tempRules[letter].Add(new Rule(letter, Code)
                    {
                        NotPrevious = NotPrevious?.ToCharArray(),
                        NotNext = NotNext?.ToCharArray(),
                        Previous = Previous?.ToCharArray(),
                        Next = Next?.ToCharArray()
                    });
                }
            }

            private static void FinalizeRules()
            {
                foreach (var pair in _tempRules)
                    _rules[pair.Key] = pair.Value.ToArray();
            }
        }

        char Prev => (_pos - 1 < 0) ? Rule.EmptyChar : _s[_pos - 1];
        char Curr => _s[_pos];
        char Next => (_pos + 1 < _s.Length) ? _s[_pos + 1] : Rule.EmptyChar;

        char[] _s;
        int _pos = -1;
        StringBuilder _phonetic;

        public static string GetPhonetic(string s)
        {
            var ph = new ColognePhonetic();
            ph.DoConversion(s);
            return ph._phonetic.ToString();
        }

        public void DoConversion(string s)
        {
            this._s = s.ToUpperInvariant().ToCharArray();
            _phonetic = new StringBuilder(s.Length + 1);
            Convert();
        }

        private bool HasNext()
        {
            ++_pos;
            return _pos < _s.Length;
        }

        private void Convert()
        {
            while (HasNext())
            {
                if (Rule.All.ContainsKey(Curr))
                {
                    var rules = Rule.All[Curr];
                    foreach (var rule in rules)
                    {
                        if (rule.Applies(Prev, Curr, Next))
                        {
                            var Code = rule.Code;
                            _phonetic.Append(rule.Code);
                            break;
                        }
                    }
                }
            }
            RemoveMultiples();
            DiscardZeroes();
        }

        /// <summary>
        /// Removes all neighbouring multiple code char occurences.
        /// </summary>
        private void RemoveMultiples()
        {
            for (int i = 0; i < _phonetic.Length; i++)
            {
                int j = i + 1;
                while (j < _phonetic.Length && _phonetic[i] == _phonetic[j])
                    ++j;
                _phonetic.Remove(i + 1, j - i - 1);
            }
        }

        /// <summary>
        /// Removes all '0' code chars except at the beginning.
        /// </summary>
        private void DiscardZeroes()
        {
            for (int i = 1; i < _phonetic.Length; i++)
                if (_phonetic[i] == '0')
                    _phonetic.Remove(i, 1);
        }

    }

}
