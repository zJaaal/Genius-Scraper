using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Knyaz.Optimus;
using Knyaz.Optimus.Dom.Interfaces;
using Knyaz.Optimus.ScriptExecuting.Jint;
using Knyaz.Optimus.TestingTools;
namespace GeniusScraper
{
    public class LyricsSearcher
    {
        private const string BaseURL = "https://genius.com/search?q={0}";
        private static Engine _engine = EngineBuilder.New().UseJint().Build();
        private static async Task<string> GetURL(string args)
            => string.Format(BaseURL, await Task.Run(() => Regex.Replace(args, @"[\s+]? | \s?", "%20").ToLower()));

        public static async Task<string> SearchForLyrics(string args)
        {
            string URL = await GetURL(args);
            var _links = await GetLinks(URL);

            foreach (IElement link in _links)
            {
                System.Console.WriteLine(link.GetAttributeNode("href").Value);
            }
            return URL;
        }
        public static async Task<IEnumerable<IElement>> GetLinks(string link)
        {
            System.Console.WriteLine($"Going to {link}");
            var page = await _engine.OpenUrl(link);
            System.Console.WriteLine("Done");
            var urls = page.Document.WaitSelector("a.mini_card");
            return urls;
        }
    }
}
