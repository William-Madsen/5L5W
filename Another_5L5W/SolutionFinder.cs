namespace Another_5L5W
{
    public class SolutionFinder
    {

        int _wordLength;
        int _numberOfWords;

        string[]? _words;
        int[] _masks;
        private int _maxValue;
        private int _progress;
        private int _lastReportedProgress;
        private readonly object _progressLock = new();

        public int FirstWords { get => _maxValue; private set; }

        private Dictionary<int, string> _bitMasksDictionary = new Dictionary<int, string>();

        public event EventHandler<string> FoundSolution;
        public event EventHandler<int> Progessbar;

        public SolutionFinder(int wordLength, int numberOfWords)
        {
            /*
            if (wordLength * numberOfWords > 26)
                throw new ArgumentOutOfRangeException("Needs more than 26 characters");
            */
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
            _masks = _bitMasksDictionary.Keys.Order().ToArray();
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
            _maxValue = _masks.Length;
            Interlocked.Exchange(ref _progress, 0);
            Interlocked.Exchange(ref _lastReportedProgress, 0);
            BitMaskFinder(0, _masks.Length, solutions, new List<int>());

            return solutions.ToArray();
        }
        private void BitMaskFinder(int usedMasks, int startIndex, List<string> solutions, List<int> words)
        {
            if (words.Count() == _numberOfWords)
            {
                var solutionWords = words.Select(mask => _bitMasksDictionary[mask]);
                var solution = string.Join(", ", solutionWords);

                solutions.Add(solution);

                OnFoundSolution(solution);
                return;
            }
            if (usedMasks == 0)
            {
                Parallel.For(0, startIndex, ctr =>
                {
                    if ((usedMasks & _masks[ctr]) == 0)
                    {
                        List<int> newWords = new List<int>(words);
                        newWords.Add(_masks[ctr]);
                        BitMaskFinder(usedMasks | _masks[ctr], ctr - 1, solutions, newWords);
                    }

                    ReportProgress(Interlocked.Increment(ref _progress));
                });
            }
            else
            {
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

        private void ReportProgress(int progress)
        {
            lock (_progressLock)
            {
                if (progress <= _lastReportedProgress)
                    return;

                _lastReportedProgress = progress;
                OnProgessbar(progress);
            }
        }

        protected virtual void OnFoundSolution(string solution)
        {
            EventHandler<string> handler = FoundSolution;
            if (handler != null)
                handler(this, solution);
        }

        protected virtual void OnProgessbar(int progress)
        {
            EventHandler<int> handler = Progessbar;
            if (handler != null)
                handler(this, progress);

        }
    }
}