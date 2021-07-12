using System;
using System.Threading.Tasks;

namespace GeniusScraper
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await LyricsSearcher.InitPuppeteer();
            while (true)
            {
                var A = Console.ReadLine();
               await LyricsSearcher.SearchForLyrics(A);
            }
        }
    }
}
