using System;
using System.Threading.Tasks;

namespace GeniusScraper
{
    class Program
    {
        static async Task Main(string[] args)
        {
            while (true)
            {
                var A = Console.ReadLine();
               await LyricsSearcher.SearchForLyrics(A);
            }
        }
    }
}
