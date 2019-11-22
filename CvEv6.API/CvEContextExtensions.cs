using CvEv6.API.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CvEv6.API
{
    public static class CvEContextExtensions
    {
        public static List<Title> GetTitles()
        {
            var titles = new List<Title>();
            Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "Data"));
            var titlesLibrary = Path.Combine(Directory.GetCurrentDirectory(), "Data\\titles.csv");
            using (var stream = File.Open(titlesLibrary, FileMode.OpenOrCreate))
            {
            }
            var sr = new StreamReader(titlesLibrary);
            while (!sr.EndOfStream)
            {
                titles.Add(new Title{ Name = sr.ReadLine()});
            }
            return titles;

        }
        public static void EnsureSeedDataForContext(this CvEContext context)
        {
            if (context.Domains.Any())
            {
                return;
            }
            context.Titles.AddRange(GetTitles());
            var domains = GetDomains();
            context.Domains.AddRange(domains);
            context.SaveChanges();
        }

        public static List<Domain> GetDomains()
        {
            Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "Data"));
            var domainsLibrary = Path.Combine(Directory.GetCurrentDirectory(), "Data\\domains.csv");
            using (var stream = File.Open(domainsLibrary, FileMode.OpenOrCreate))
            {
            }
            var sr = new StreamReader(domainsLibrary);


            var domains = new List<Domain>();
            using (sr)
            {
                while (!sr.EndOfStream)
                {
                    var domainName = sr.ReadLine().Trim();
                    var docs = sr.ReadLine().LineToArray().ToList();
                    var docsToList = new List<Document>();
                    foreach (string doc in docs)
                    {
                        docsToList.Add(new Document { Name = doc });
                    }
                    domains.Add(new Domain { Name = domainName, Documents = docsToList });
                }
            }
            return domains;
        }

        public static string[] LineToArray(this string item)
        {
            string[] delimiters = { ","};
            StringSplitOptions options = StringSplitOptions.RemoveEmptyEntries;
            var items = item.Split(delimiters, options);
            for (int i = 0; i < items.Length; i++)
                items[i] = items[i].Trim();
            return items;
        }

    }
}
