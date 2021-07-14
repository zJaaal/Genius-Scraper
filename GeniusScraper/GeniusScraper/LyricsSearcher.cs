using System.Collections.Generic;
using System.Linq;
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
            var doc = page.Document;

            var urls = page.Document.WaitSelector("#search_results_label text_label text_label--x_small_text_size text_label--gray search_results_label--no_horizontal_padding mini-song-card");
            return urls;

            //var page = await _engine.OpenUrl("https://html5test.com");

            //Wait until it finishes the test of browser and get DOM element with score value.
            var tagWithValue = page.Document.WaitSelector("#score strong");
            return tagWithValue;
        }
    }
}
