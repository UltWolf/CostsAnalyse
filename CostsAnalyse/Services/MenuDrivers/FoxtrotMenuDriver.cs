using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace CostsAnalyse.Services.MenuDrivers
{
    public class FoxtrotMenuDriver
    {
        BinaryFormatter bf = new BinaryFormatter();
        public void GetPagesAuto()
        {
            WebRequest WR = WebRequest.Create("https://www.foxtrot.com.ua/?gclid=CjwKCAjw__fnBRANEiwAuFxET_FFLTex-3uI9ezpGqetdLABorGdY-_z-sXTQKlgrdCnQVACZYDGaxoCQ3cQAvD_BwE");
            WR.Method = "GET";
            string html = "";
            using (var response = WR.GetResponse())
            {
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    html = sr.ReadToEnd();
                }
            }
            HtmlParser parser = new HtmlParser();
            var parsedDocument = parser.ParseDocument(html);
            var elementWithMenu = parsedDocument.GetElementsByClassName("catalog-submenu")[0];
            var liFromMenu = elementWithMenu.GetElementsByTagName("li");
            HashSet<string> hrefs = new HashSet<string>();
            foreach (var li in liFromMenu)
            {
                if (li.ClassName != "category-list-button")
                {
                    var wrapper = li.GetElementsByClassName("category-menu-wrapper");
                    if (wrapper.Length > 0)
                    {
                        var columns = wrapper[0].GetElementsByClassName("category-column");
                        foreach (var column in columns)
                        {
                            var items = column.GetElementsByClassName("category-item");
                            foreach (var item in items)
                            {
                                var ul = item.GetElementsByTagName("ul");
                                if (ul.Length > 0)
                                {
                                    GetHrefsFromList(ul[0], ref hrefs);
                                }
                            }
                        }
                    }
                    else
                    {
                        GetHrefsFromA(li);
                    }
                }
            }
            using (FileStream fs = new FileStream("FoxtrotHrefs.txt", FileMode.Create, FileAccess.Write))
            {
                bf.Serialize(fs, hrefs);
            }

        }
        public void GetHrefsFromList(IElement ulItem, ref HashSet<string> hrefs)
        {
            foreach (var listItem in ulItem.GetElementsByTagName("li"))
            {
                var element = GetHrefsFromA(listItem);
                if (element != null)
                {
                    hrefs.Add(element);
                }

            }
        }
        public string GetHrefsFromA(IElement listItem)
        {

            var a = listItem.GetElementsByTagName("a");
            if (a.Length > 0)
            {
                if (!IsAllCategoriesHref(a[0]))
                {
                    return a[0].GetAttribute("href");
                }

            }
            return null;

        }
        public bool IsAllCategoriesHref(IElement a)
        {
            return (a.GetElementsByClassName("all-categories-text").Length > 0);
        }
    }
}
