using AngleSharp.Html.Parser;
using CostsAnalyse.Extensions;
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
        public static   void SerialiseProxyServers(bool IsForce) {
            if (File.Exists("proxyList.txt") || IsForce)
            {
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
                using (FileStream fs = new FileStream("proxyList.txt", FileMode.Create, FileAccess.Write))
                {

                    bf = new BinaryFormatter();
                    bf.Serialize(fs, proxyList);
                }
            }
        }
        public static void SerialiseProxyServersUA(bool IsForce){
             if (File.Exists("proxyList.txt") || IsForce)
            {
                List<String> proxyList = new List<string>();
                var httpRequest = (HttpWebRequest)HttpWebRequest.Create("https://www.proxynova.com/proxy-server-list/country-ua/");
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
                var table = body.GetElementById("tbl_proxy_list");

                var trs = table.GetElementsByTagName("tr");
                for (int i = 1; i < trs.Length; i++)
                {
                    var tds = trs[i].GetElementsByTagName("td");
                    if (tds.Length > 0)
                    {
                        var proxyScript = tds[0].GetElementsByTagName("script");
                        if (proxyScript.Length != 0)
                        {
                            var arrayWithData = proxyScript[0].TextContent.Split("\'");
                            if (arrayWithData.Length > 4)
                            {
                                var numb = arrayWithData[1].Substring(8);
                                var lastDataNumbs = arrayWithData[3];
                                string http = numb + lastDataNumbs;
                                string port = tds[1].TextContent.RemoveWebGarbage();
                                proxyList.Add(http + ":" + port);
                            }
                        }
                    }
                    
                }
                using (FileStream fs = new FileStream("proxyList.txt", FileMode.Create, FileAccess.Write))
                {

                    bf = new BinaryFormatter();
                    bf.Serialize(fs, proxyList);
                }
            }
        }
        public static void SerializeByPuts(List<string> hrefs) {
            using (FileStream fs = new FileStream("proxyList.txt", FileMode.Create, FileAccess.Write))
            {

                bf = new BinaryFormatter();
                bf.Serialize(fs, hrefs);
            }
        }
        
        public static List<string> GetProxyHrefs() {
            List<string> hrefs = new List<string>();
            using (FileStream fs = new FileStream("proxyList.txt", FileMode.Open, FileAccess.Read)){
                bf = new BinaryFormatter();
                hrefs = (List<string>)bf.Deserialize(fs);
            }
            return hrefs;
        }

        }
}
