using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ParseAutoSup
{
    class Program
    {
        class Car
        {
            public string Name { get; set; }
            public string Href { get; set; }

            public Car(string name, string href)
            {
                Name = name;
                Href = href;
            }
        }

        class Model
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public string Href { get; set; }
            public Car Car { get; set; }
        }

        static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Console.WriteLine("Hello World!");
            Parse();
            Console.ReadLine();
        }

        static async void Parse()
        {
            HttpClient http = new HttpClient();
            string url = "https://autosup.by";
            var response = await http.GetAsync(url);
            var pageContent = await response.Content.ReadAsStringAsync();
            HtmlDocument documentHome = new HtmlDocument();
            documentHome.LoadHtml(pageContent);
            var nodesCars = documentHome.DocumentNode.SelectNodes("//table[contains(@class, 't_m_t')]//td//a[contains(@class, 'bluelink11')]");
            List<Car> cars = new List<Car>();
            foreach (var node in nodesCars)
            {
                Car car = new Car(node.InnerText, node.Attributes["href"].Value);
                cars.Add(car);
                response = await http.GetAsync(url + car.Href);
                pageContent = await response.Content.ReadAsStringAsync();
                HtmlDocument documentCar = new HtmlDocument();
                documentCar.LoadHtml(pageContent);
                var nodesModels = documentCar.DocumentNode.SelectNodes("//tr[contains(@class, 'brd')]");
                Console.WriteLine(nodesModels.Count);
            }
            foreach (Car s in cars)
            {
                Console.WriteLine($"{s.Name} {s.Href}");
            }
            //Console.WriteLine(nodes.Count);
            //Console.WriteLine(nodes[1].InnerText);
        }
    }
}
