using System;
using System.Threading.Tasks;
using Serilog;

namespace GeniusScraper
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.File("logs/.log")
                .WriteTo.Console()
                .CreateLogger();

            await LyricsSearcher.InitPuppeteer();
            while (true)
            {
               var A = Console.ReadLine();
               await LyricsSearcher.SearchForLyrics(A);
            }
        }
    }
}
