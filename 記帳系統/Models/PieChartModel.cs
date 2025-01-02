using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 記帳系統.Models
{
    public class PieChartModel
    {
        public List<string> ConditionTypes { get; set; }
        public List<string> AnalyzeTypes { get; set; }

        public List<RawData> RawData { get; set; }

        public PieChartModel()
        {
            ConditionTypes = new List<string>();
            AnalyzeTypes = new List<string>();
            RawData = new List<RawData>();
        }
    }
}
