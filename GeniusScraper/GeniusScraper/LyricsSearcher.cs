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

namespace GeniusScraper
{
    public class LyricsSearcher
    {
        private const string BaseURL = "https://genius.com/search?q={0}";
        private static LaunchOptions Options = new()
        {
            Headless = true,
            ExecutablePath = Path.Combine("C:", "Programs Files", "Google", "Chrome", "Application", "chrome.exe"),
            Product = Product.Chrome
        };
        private static Browser browser { get; set; }
        private static Page page { get; set; }
        private static async Task<string> GetURL(string args)
            => string.Format(BaseURL, await Task.Run(() => Regex.Replace(args, @"[\s+]? | \s?", "%20").ToLower()));

        public static async Task<string> SearchForLyrics(string args)
        {
            string Link = await GetURL(args);
            List<string> Links = await GetLinks(Link);

            foreach (string link in Links)
            {
                Console.WriteLine(link);
            }
            return Link;
        }
        public static async Task InitPuppeteer()
        {
            Console.WriteLine("Initializing Puppeteer...");

            Console.WriteLine("Initializing Browser...");
            browser = await Puppeteer.LaunchAsync(Options, null);

            Console.WriteLine("Initializing Page...");
            page = await browser.NewPageAsync();
        }
        public static async Task<List<string>> GetLinks(string link)
        {
            page = await browser.NewPageAsync();
            Console.WriteLine($"Going to {link}");
            await page.GoToAsync(link);

            var Links = @"Array.from(document.querySelectorAll('a.mini_card')).map(a => a.href);";

            Console.WriteLine("Executing Query...");
            var urls = await page.EvaluateExpressionAsync<string[]>(Links);

            Console.WriteLine("Done");
            var MyUrls = urls.Where(Url => Url.Contains("-lyrics")).ToList();
            return MyUrls;
        }
    }
}
