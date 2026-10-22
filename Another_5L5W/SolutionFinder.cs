using System.Security.Cryptography;

namespace Another_5L5W
{
    public class SolutionFinder
    {
        int _wordLength;
        int _numberOfWords;

        string[] _words;

        public SolutionFinder(int wordLength, int numberOfWords)
        {
            if (wordLength * numberOfWords > 26)
                throw new ArgumentOutOfRangeException("Kræver mere end 26 bogstaver");
            _numberOfWords = numberOfWords;
            _wordLength = wordLength;
        }

        public int LoadWords(string file)
        {
            _words = File.ReadAllLines(file);
            _words = _words.Where(x => x.Length == _wordLength && x.Distinct().Count() == _wordLength).ToArray();
            return _words.Length;
        }


        public string[] FindSolutions()
        {
            int count = 0;
            int processingCount = 0;
            List<string> solutions = new List<string>();
            RecurviveFind("", _words.Length - 1, solutions);
            /*for (var a = _words.Length - 1; a >= 0; a--)
            {
                for (var b = a - 1; b >= 0; b--)
                {
                    var ab = _words[a] + _words[b];
                    if (ab.Distinct().Count() != 2 * _wordLength)
                        continue;

                    for (var c = b - 1; c >= 0; c--)
                    {
                        var abc = _words[a] + _words[b] + _words[c];
                        if (abc.Distinct().Count() != 3 * _wordLength)
                            continue;

                        for (var d = c - 1; d >= 0; d--)
                        {
                            var abcd = _words[a] + _words[b] + _words[c] + _words[d];
                            if (abcd.Distinct().Count() != 4 * _wordLength)
                                continue;

                            for (var e = d - 1; e >= 0; e--)
                            {
                                var abcde = _words[a] + _words[b] + _words[c] + _words[d] + _words[e];
                                if (abcde.Distinct().Count() != 5 * _wordLength)
                                    continue;
                                count++;
                                processingCount++;
                                solutions.Add(abcde);
                                if (processingCount == 10)
                                {
                                    Console.WriteLine("Processing...");
                                }

                            }
                        }
                    }
                }
            }*/
            return solutions.ToArray();
        }

        private void RecurviveFind(string usedWords, int startIndex, List<string> solutions)
        {
            //Exit?
            if( usedWords.Length == _numberOfWords*_wordLength)
            {
                for(var a = 1; a < _numberOfWords; a++)
                {
                    usedWords = usedWords.Insert(a * _wordLength + (a - 1), " ");
                }
                solutions.Add(usedWords);
                return;
            }

            //Find
            for(var i = startIndex; i >= 0; i--)
            {
                string joinedWords = string.Join("", usedWords, _words[i]);
                if (joinedWords.Distinct().Count() != joinedWords.Length)
                    continue;
                RecurviveFind(joinedWords, i - 1, solutions);
            }
        }
    }
}