using System;
using System.Threading.Tasks;

namespace KölnerPhonetik
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] words = {
                "Flensburg Siedlung Uckerblick",
                "Langförden Lindwallstraße",
                "Hentrup Kohlenmarkt",
                "Polch St2114",
                "Heilbronn Leidenharterhof",
                "Köhra Langen Ellern",
                "Kitzen Rodecker Straße",
                "Kitzen Rodecker Strasse",
                "Kitzen Rodeker Straße",
                "Kitzen Rodekker Straße",
                "Müller-Lüdenscheidt",
                "Müler-Luedenscheidt",
                "Meyer",
                "Mayr",
                "Meier",
                "Maier",
                "Meir",
        };
            foreach (var w in words)
                Console.WriteLine($"{ColognePhonetic.GetPhonetic(w)} \t\t {w}");

            Console.ReadLine();
        }
    }

}
