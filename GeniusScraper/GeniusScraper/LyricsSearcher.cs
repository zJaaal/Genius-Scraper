using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Net;
using HtmlAgilityPack;
using PuppeteerSharp;
using System.IO;
using Serilog;

namespace GeniusScraper
{
    public class LyricsSearcher
    {
        private const string BaseURL = "https://genius.com/search?q={0}";
        private static LaunchOptions Options = new()
        {
            Headless = true,
            ExecutablePath = Path.Combine("/usr", "lib", "firefox", "firefox"),
            Product = Product.Firefox
        };
        private static Browser browser { get; set; }
        private static Page page { get; set; }
        private static async Task<string> GetURL(string args)
            => string.Format(BaseURL, await Task.Run(() => Regex.Replace(args, @"[\s+]? | \s?", "%20").ToLower()));

        public static async Task<string> SearchForLyrics(string args)
        {
            string Link = await GetURL(args);
            List<string> Links = await GetLinks(Link);

            Log.Debug("Searching for links");
            foreach (string link in Links)
                Log.Verbose($"Link: {link}"); //Esto es innecesario, y si el nivel de loggeo no es Verbose, es procesamiento perdido

            return Link;
        }
        public static async Task InitPuppeteer()
        {
            Log.Information("Initializing Puppeteer");
            Log.Debug("Initializing Browser");
            browser = await Puppeteer.LaunchAsync(Options, null);

            Log.Debug("Initializing Page");
            page = await browser.NewPageAsync();
        }
        public static async Task<List<string>> GetLinks(string link)
        {
            page = await browser.NewPageAsync();
            Log.Debug($"Navigating to {link}");
            await page.GoToAsync(link);

            var Links = @"Array.from(document.querySelectorAll('a.mini_card')).map(a => a.href);";

            Log.Debug("Querying the page");
            var urls = await page.EvaluateExpressionAsync<string[]>(Links);
            var MyUrls = urls.Where(Url => Url.Contains("-lyrics")).ToList();
            return MyUrls;
        }
    }
}