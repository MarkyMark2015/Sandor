using System;
using System.Diagnostics;

namespace SqlFulltextTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var t = new Tester();
            // DB füllen 
            // t.FillDb();

            // Test Abfrage
            //RunTestSerie(t.TestQueryLikeWildcard);
            //RunTestSerie(t.TestQueryWildcardLike);
            //RunTestSerie(t.TestQueryFulltext);

            // Interaktive Abfrage
            Console.WriteLine("Enter pattern");
            while (true)
            {
                var s = Console.ReadLine();
                if (s == string.Empty) break;
                RunTest( () => t.TestQueryFulltextInteractive(s));
            }
        }

        static void RunTestSerie(Action act)
        {
            for (int i=0; i<1; i++)
            {
                RunTest(act);
            }
        }

        static void RunTest(Action act)
        {
            var sw = Stopwatch.StartNew();
            act();
            sw.Stop();
            var elapsed = sw.Elapsed;
            Console.WriteLine($"OK, elapsed {elapsed.TotalMilliseconds:#,##0} ms ");
        }

    }
}
