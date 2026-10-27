using Another_5L5W;
using System.Diagnostics;

namespace Another_5W5L_Console
{
    public class Collect
    {
        private static int _wordsLoaded = 0;
        private static int _progress = 0;

        public static void Main()
        {
            var solutionFinder = new SolutionFinder(5, 5);
            solutionFinder.FoundSolution += FoundSolutionWriter;
            solutionFinder.Progessbar += ProgressBar;
            _wordsLoaded = solutionFinder.LoadWords(@"C:\Users\temp\source\repos\Wpf_app\Another_5L5W\List\words_beta.txt");
            Console.WriteLine("{0} words found", _wordsLoaded);
            string[] solutions = solutionFinder.FindSolutions();

            Console.WriteLine("Found {0} solutions", solutions.Length);

        }

        private static void ProgressBar(object? sender, int e)
        {
            _progress = e;
            Console.Write("\r[{0} of {1}]...", e, _wordsLoaded);
        }

        private static void FoundSolutionWriter(object? sender, string e)
        {
            Console.WriteLine("\r[{0} of {1}] : {2}", _progress, _wordsLoaded, e);
        }
    }
}