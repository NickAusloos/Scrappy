using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;
using HtmlAgilityPack;
using ScrapySharp.Extensions;
using ScrapySharp.Network;

namespace Scrappy
{
    class Program
    {
        static List<KeyValuePair<string, string>> siteScraper = new List<KeyValuePair<string, string>>();
        static void Main(string[] args)
        {
            siteScraper.Add(new KeyValuePair<string, string>("http://en.chateauversailles.fr/plan-your-visit/practical-information#high-season", "attendance"));
            siteScraper.Add(new KeyValuePair<string, string>("https://www.louvre.fr/en/hours-admission-directions/hours#tabs", "#wysiwyg"));    // hours in paragraph with day details
            siteScraper.Add(new KeyValuePair<string, string>("https://www.toureiffel.paris/en/rates-opening-times", ".time-value"));            // hours directly 
            
            Stopwatch stopWatch = new Stopwatch(); 
            stopWatch.Start();
            ScrapingBrowser browser = new ScrapingBrowser();
            WebPage page = browser.NavigateToPage(new Uri(siteScraper[0].Key));
            HtmlNode node = page.Html.CssSelect(siteScraper[0].Value).First();
            stopWatch.Stop();
            Console.WriteLine(GetIndexOfOccurence(node.InnerText));
            //Console.WriteLine(node.InnerText.Trim());
            Console.WriteLine(stopWatch.ElapsedMilliseconds);
        }

        private static string DayOfWeek()
        {
            return DateTime.Now.ToString("dddd", new CultureInfo("en-EN"));
        }

        private static string GetIndexOfOccurence(string text)
        {
            int indexDay = text.IndexOf(DayOfWeek());

            if (indexDay != -1)
            {
                int indexNode = text.IndexOf("\n\t", indexDay);                        // <br>
                if (indexNode != -1)
                    return text.Substring(indexDay, indexNode - indexDay);
            }
            //text.Substring(indexDay, )

            return "";
        }
    }
}
