using Another_5L5W;
using System.ComponentModel;
using System.Diagnostics;

namespace Another_5W5L_Console
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            var solutionFinder = new SolutionFinder(5, 5);
            solutionFinder.FoundSolution += FoundSolutionWriter;

            int wordsLoaded = solutionFinder.LoadWords(@"C:\Users\temp\source\repos\Wpf_app\Another_5L5W\List\words_alpha.txt");
            string[] solutions = solutionFinder.FindSolutions();

            /*foreach (var word in solutions)
            {
                Console.WriteLine(word);
            }*/

            stopWatch.Stop();
            Console.WriteLine("{0} words found", wordsLoaded);
            Console.WriteLine("Found {0} solutions", solutions.Length);
            Console.WriteLine(stopWatch);

        }

        private static void FoundSolutionWriter(object? sender, string e)
        {
            Console.WriteLine(e);
        }
    }
}