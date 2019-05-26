using AngleSharp.Html.Parser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace CostsAnalyse.Services.ProxyServer
{
    public class ProxyServerConnectionManagment
    {
        private static BinaryFormatter bf;
        public static void SerialiseProxyServers() {
            List<String> proxyList = new List<string>();
                var httpRequest = (HttpWebRequest)HttpWebRequest.Create("http://foxtools.ru/Proxy?country=RU&al=True&am=True&ah=True&ahs=True&http=True&https=True");
                httpRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                httpRequest.KeepAlive = true;
                httpRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:65.0) Gecko/20100101 Firefox/65.0";

                var httpResponse = httpRequest.GetResponse();
                string html = "";
                using (Stream stream = httpResponse.GetResponseStream())
                {
                    using (StreamReader sr = new StreamReader(stream))
                    {
                        html = sr.ReadToEnd();
                    }
                }
                var parser = new HtmlParser();
                var body = parser.ParseDocument(html);
                var table = body.GetElementById("theProxyList");

                var trs = table.GetElementsByTagName("tr");
                for (int i = 1; i < trs.Length; i++)
                {
                    var tds = trs[i].GetElementsByTagName("td");
                    string proxy = tds[1].TextContent;
                    string port = tds[2].TextContent;
                    proxyList.Add(proxy + ":" + port);
                }
            using (FileStream fs = new FileStream("", FileMode.Create, FileAccess.Write)) {

                bf = new BinaryFormatter();
                bf.Serialize(fs, proxyList);
                }
        }
        public static List<string> GetProxyHrefs() {
            List<string> hrefs = new List<string>();
            using (FileStream fs = new FileStream("", FileMode.Open, FileAccess.Read)){
                bf = new BinaryFormatter();
                hrefs = (List<string>)bf.Deserialize(fs);
            }
            return hrefs;
        }

        }
}
