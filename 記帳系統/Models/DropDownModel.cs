using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 記帳系統.Models
{
    public static class DropDownModel
    {
        public static Dictionary<string, List<string>> Types = new Dictionary<string, List<string>>()
        {
            { "用餐", GetFoodItems() },
            { "交通" ,GetTrafficItems()},
            { "租金" ,GetRentItems()},
            { "治裝" ,GetDressItems()},
            { "娛樂" ,GetEntertainmentItems()},
            { "學習" ,GetLearningItems()},
            { "投資" ,GetInvestmentItems()},
        };



        public static List<string> GetFoodItems()
        {




            return new List<string>
        {
            "早餐",
            "午餐",
            "晚餐",
            "下午茶",
            "霄夜"
        };
        }

        public static List<string> GetTrafficItems()
        {
            return new List<string>
        {
            "公車",
            "捷運",
            "腳踏車租借",
            "計程車",
            "汽/機車油費",
            "機票"
        };
        }

        public static List<string> GetRentItems()
        {
            return new List<string>
        {
            "租金"
        };
        }

        public static List<string> GetDressItems()
        {
            return new List<string>
        {
            "衣服鞋襪",
            "理髮",
            "化妝"
        };
        }

        public static List<string> GetEntertainmentItems()
        {
            return new List<string>
        {
            "電影",
            "音樂會",
            "電子遊戲",
            "影音訂閱"
        };
        }

        public static List<string> GetLearningItems()
        {
            return new List<string>
        {
            "線上課程",
            "家教"
        };
        }

        public static List<string> GetInvestmentItems()
        {
            return new List<string>
        {
            "股票",
            "債券",
            "基金",
            "期貨",
            "其他"
        };
        }
    }
}
