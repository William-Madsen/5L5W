using System.Runtime.InteropServices;
using System.Security.Cryptography;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Another_5L5W
{
    public class SolutionFinder
    {

        int _wordLength;
        int _numberOfWords;

        string[]? _words;
        int[] _masks;
        private Dictionary<int, string> _bitMasksDictionary = new Dictionary<int, string>();

        public SolutionFinder(int wordLength, int numberOfWords)
        {
            if (wordLength * numberOfWords > 26)
                throw new ArgumentOutOfRangeException("Needs more than 26 characters");
            _numberOfWords = numberOfWords;
            _wordLength = wordLength;
        }


        public int LoadWords(string file)
        {
            _words = File.ReadAllLines(file);
            _words = _words.Where(x => x.Length == _wordLength && x.Distinct().Count() == _wordLength).ToArray();

            if (_words == null)
                throw new NullReferenceException("No words loaded");

            foreach (var word in _words)
            {
                _bitMasksDictionary.TryAdd(wordToMask(word), word);
            }
            Console.WriteLine("Masked: {0}", _bitMasksDictionary.Count());
            _masks = _bitMasksDictionary.Keys.Order().ToArray();
            /*
            Console.WriteLine("Masks: {0:B26} => word: {1}", _masks[5], bitMasksDictionary[_masks[5]]);
            Console.WriteLine("Masks: {0:B26} => word: {1}", _masks[6], bitMasksDictionary[_masks[6]]);
            Console.WriteLine("Masks: {0:B26} => word: {1}", _masks[7], bitMasksDictionary[_masks[7]]);
            Console.WriteLine("Masks: {0:B26} => word: {1}", _masks[8], bitMasksDictionary[_masks[8]]);
            Console.WriteLine("Masks: {0:B26} => word: {1}", _masks[9], bitMasksDictionary[_masks[9]]);
            */
            return _words.Length;
        }

        private int wordToMask(string word)
        {
            int mask = 0;
            foreach (char ch in word)
            {
                mask |= 1 << (ch - 'a');
            }
            return mask;
        }


        public string[] FindSolutions()
        {
            List<string> solutions = new List<string>();
            BitMaskFinder(0, _masks.Length - 1, solutions, new List<int>());

            return solutions.ToArray();
        }


        private void BitMaskFinder(int usedMasks, int startIndex, List<string> solutions, List<int> words)
        {
            if (words.Count() == _numberOfWords)
            {
                //TODO: add solutions
                Console.WriteLine("{0:b26}", usedMasks);
                foreach (var mask in words)
                {
                    Console.WriteLine(_bitMasksDictionary[mask]);
                }
                return;
            }

            for (var i = startIndex; i >= 0; i--)
            {
                if ((usedMasks & _masks[i]) == 0)
                {
                    List<int> newWords = new List<int>(words);
                    newWords.Add(_masks[i]);
                    BitMaskFinder(usedMasks | _masks[i], i - 1, solutions, newWords);
                }
            }
        }
    }
}