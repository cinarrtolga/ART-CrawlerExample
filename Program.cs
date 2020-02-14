using System.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

using HtmlAgilityPack;

namespace ART_CrawlerExample
{
    class Program
    {
        static void Main(string[] args)
        {
            string requestUrl = Console.ReadLine();

            var result = Task.Run(async () => await GetHtmlPage(requestUrl)).Result;

            Task.Run(async () => await GetSearchParameter(result, "meta", "content"));

            Task.Run(async () => await GetSearchParameter(result, "a", "href"));

            Task.Run(async () => await SearchKeywordInPage(result, "a", "href", "react"));

            Console.ReadLine();
        }

        public static async Task<HtmlDocument> GetHtmlPage(string pageUrl)
        {
            var httpClient = new HttpClient();
            var htmlDocument = new HtmlDocument();

            htmlDocument.LoadHtml(await httpClient.GetStringAsync(pageUrl));

            return htmlDocument;
        }

        public static async Task GetSearchParameter(HtmlDocument targetDocument, string targetTag, string targetAttribute)
        {
            var targetElementList = targetDocument.DocumentNode.Descendants(targetTag).ToList();

            await Task.Run(() =>
            {
                foreach (var item in targetElementList)
                {
                    if (!string.IsNullOrEmpty(item.InnerText.Trim()) || item.Attributes[targetAttribute] != null)
                    {
                        Console.WriteLine(item.Attributes[targetAttribute].Value.ToString());
                    }
                }
            });
        }

        public static async Task SearchKeywordInPage(HtmlDocument targetDocument, string targetTag, string targetAttribute, string searchKey)
        {
            var targetElementList = targetDocument.DocumentNode.Descendants(targetTag).ToList();

            await Task.Run(() =>
            {
                foreach (var item in targetElementList)
                {
                    if ((!string.IsNullOrEmpty(item.InnerText.Trim()) || item.Attributes[targetAttribute] != null)
                            && item.Attributes[targetAttribute].Value.Contains(searchKey))
                    {
                        Console.WriteLine(item.Attributes[targetAttribute].Value.ToString());
                    }
                }
            });
        }
    }
}
