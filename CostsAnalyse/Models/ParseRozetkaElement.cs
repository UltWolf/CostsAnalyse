using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CostsAnalyse.Models
{
    
        public class ParseRozetkaElement
        {
            public int code { get; set; }
            public Content[] content { get; set; }
        }

        public class Content
        {
            public int code { get; set; }
            public Content1 content { get; set; }
        }

        public class Content1
        {
            public string title { get; set; }
            public string title_only { get; set; }
            public string promo_title_part { get; set; }
            public int product_id { get; set; }
            public string price { get; set; }
            public int seller_id { get; set; }
            public int merchant_id { get; set; }
            public int hide_wrapper_class { get; set; }
            public Image image { get; set; }
            public Large_Image large_image { get; set; }
            public Large large { get; set; }
            public string sell_status { get; set; }
            public string status { get; set; }
            public bool isActive { get; set; }
            public int id { get; set; }
            public string href { get; set; }
            public string href_ua { get; set; }
            public float price_usd { get; set; }
            public int old_price { get; set; }
            public string unit { get; set; }
            public int parent_id { get; set; }
            public string producer { get; set; }
            public int producer_id { get; set; }
            public float users_rating { get; set; }
            public string count_comments { get; set; }
            public string count_marks { get; set; }
            public string comments_href { get; set; }
            public string top_parent_id { get; set; }
            public string docket { get; set; }
            public string tag { get; set; }
            public string name { get; set; }
            public string mpath { get; set; }
            public string parent_title { get; set; }
            public string parent_title_ua { get; set; }
            public int group_id { get; set; }
            public string state { get; set; }
            public int pl_bonus_charge_pcs { get; set; }
            public string pl_use_instant_bonus { get; set; }
            public Prices prices { get; set; }
            public object[] special_prices { get; set; }
        }

        public class Image
        {
            public string url { get; set; }
            public int width { get; set; }
            public int height { get; set; }
        }

        public class Large_Image
        {
            public string url { get; set; }
            public int width { get; set; }
            public int height { get; set; }
        }

        public class Large
        {
            public string url { get; set; }
            public int width { get; set; }
            public int height { get; set; }
        }

        public class Prices
        {
            public string _2360 { get; set; }
            public string recommended_price { get; set; }
            public string _8151 { get; set; }
            public string _actions { get; set; }
        } 
}
