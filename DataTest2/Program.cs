using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSVHelper;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace DataTest2
{
    internal class Program
    {
        static void Main(string[] args)
        {

            Console.OutputEncoding = System.Text.Encoding.UTF8;

            string filePath = @"C:\CSharp練習\data read\MOCK_DATA11.csv";

            long memoryBefore = GC.GetTotalMemory(true);
            Stopwatch stopwatch = Stopwatch.StartNew();
            List<CsvRow> result = CSVHelper.CSV.ReadCSV<CsvRow>(filePath, true);
            // 停止計時
            stopwatch.Stop();

            // 獲取記憶體用量
            long memoryAfter = GC.GetTotalMemory(true);
            double memoryUsed = (memoryAfter - memoryBefore) / 1024.0 / 1024.0; // 轉換為 MB

            // 顯示結果
            Console.WriteLine($"檔案名稱: {Path.GetFileName(filePath)}");
            Console.WriteLine($"資料筆數: {result.Count}");
            Console.WriteLine($"讀取花費時間: {stopwatch.Elapsed.TotalSeconds:F4} 秒");
            Console.WriteLine($"耗費記憶體: {memoryUsed:F2} MB");

            Console.ReadLine();
        }
    }
}
