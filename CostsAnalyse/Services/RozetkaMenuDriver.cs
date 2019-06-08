using AngleSharp.Html.Parser;
using CostsAnalyse.Models;
using CostsAnalyse.Services.ProxyServer;
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
        List<string> proxyList;
        public RozetkaMenuDriver() {
            proxyList = ProxyServerConnectionManagment.GetProxyHrefs();
        }
      
        
        public void getPages() {
            HashSet<string> pages = new HashSet<string>();
            pages.Add("https://rozetka.com.ua/ua/notebooks/c80004/filter/");
            pages.Add("https://rozetka.com.ua/ua/tablets/c130309/filter/");
            pages.Add("https://rozetka.com.ua/ua/notebook-bags/c80036/");
            using (FileStream fs = new FileStream("RozetkaHrefs.txt", FileMode.Create, FileAccess.Write))
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fs, pages);
            }
        }

       public void GetPagesAuto(){
           HashSet<String> listOfHrefs = new HashSet<string>();
           ThreadDelay.Delay();
           WebRequest webRequest = WebRequest.Create("https://rozetka.com.ua/ua/all-categories-goods/");
           string html = "";
            using (var response = webRequest.GetResponse())
            {
                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    html =  streamReader.ReadToEnd();
                }
            }
            HtmlParser parser = new HtmlParser();
            var htmlDocument = parser.ParseDocument(html);
            var bodyDiv= htmlDocument.GetElementsByClassName("all-cat-content");
            var hrefs = bodyDiv[0].GetElementsByTagName("a");
            foreach(var href in hrefs){try{
                string fullHref= href.GetAttribute("href");
                webRequest = WebRequest.Create(fullHref);
                using(var response= webRequest.GetResponse()){
                  using(StreamReader streamReader = new StreamReader(response.GetResponseStream())){
                     html =   streamReader.ReadToEnd();
                     htmlDocument = parser.ParseDocument(html);
                    
                     if(htmlDocument.GetElementById("block_with_goods")!=null){
                         listOfHrefs.Add(fullHref);
                     }else{
                        var portal =  htmlDocument.GetElementsByClassName("portal-automatic");
                        if(portal.Length!=0){
                        var hrefsFromPortal = portal[0].GetElementsByTagName("a");
                        foreach(var hrefFromPortal in hrefsFromPortal ){
                            listOfHrefs.Add(hrefFromPortal.GetAttribute("href"));
                        }
                        }
                        }
                  }
                }
            }
                        catch(Exception ex){}
                        }
                     
                using (FileStream fs = new FileStream("RozetkaHrefs.txt", FileMode.Create, FileAccess.Write))
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fs,listOfHrefs.ToList());
            }

        }
    }
}
