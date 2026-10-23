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

            int wordsLoaded = solutionFinder.LoadWords(@"C:\Users\temp\source\repos\Wpf_app\Another_5L5W\List\words_beta.txt");
            Console.WriteLine("{0} words found", wordsLoaded);
            string[] solutions = solutionFinder.FindSolutions();
            Console.WriteLine("Found {0} solutions", solutions.Length);
            
            foreach (var word in solutions)
            {
                Console.WriteLine(word);
            }

            stopWatch.Stop();
            Console.WriteLine(stopWatch);
        }
    }
}