using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 記帳系統.Models;

namespace 記帳系統.Utility.Strategy.Interface
{
    public interface IGroupingStrategy
    {
        List<AnalysisModel> GroupData(List<AnalysisRawDataDAO> rawData, List<string> conditionTypes, List<string> analyzeTypes);
    }
}
