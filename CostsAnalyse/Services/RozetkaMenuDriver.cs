using AngleSharp.Html.Parser;
using CostsAnalyse.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace CostsAnalyse.Services
{
    public class RozetkaMenuDriver
    {
        private List<String> Parses()
        {
            List<String> htmls = new List<string>();
            WebRequest webRequest = WebRequest.Create("https://catalog-api.rozetka.com.ua/v2/fats/getFullMenu?lang=ru");
            WebResponse response = webRequest.GetResponse(); 
            using (Stream stream = response.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    string jsonResult = reader.ReadToEnd();
                    string[] compredoResult = jsonResult.Split(",");
                    foreach (var compredo in compredoResult) {
                        var elements = compredo.Split(":");
                        for(int i = 0; i<elements.Length; i++) {
                            if (elements[i].Contains("manual_url"))
                            {
                                string url = elements[++i] +":"+ elements[++i];
                                    htmls.Add(url.Replace("\"","")); 
                             }
                        }
                    }
                    
                }
            } 
            response.Close();
            return htmls;
        }
        
        public void getPages() {
            HashSet<string> pages = new HashSet<string>();
            var htmls = Parses();
            foreach (var html in htmls) {
                try
                {
                    WebRequest webRequest = WebRequest.Create(html);
                    WebResponse response = webRequest.GetResponse();
                    using (Stream stream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            string page = reader.ReadToEnd();
                            HtmlParser parser = new HtmlParser();
                            var DomDocument = parser.ParseDocument(page);

                            var menu = DomDocument.GetElementById("menu_categories_left");
                            if (menu != null)
                            {
                                foreach (var child in menu.Children)
                                {
                                    var href = child.GetElementsByTagName("a")[0];
                                    pages.Add(href.GetAttribute("href"));
                                }
                            }
                        }
                    }
                    response.Close();
                }
                catch (Exception ex) { }
            }
            using (FileStream fs = new FileStream("RozetkaHrefs.txt", FileMode.Create, FileAccess.Write))
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fs, pages);
            }
        } 
    }
}
