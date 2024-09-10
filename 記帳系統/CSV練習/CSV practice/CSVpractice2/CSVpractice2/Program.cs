using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace CSVpractice2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string directoryPath = @"C:\CSharp練習\記帳系統\記帳系統\CSV練習\CSV practice\CSV practice";
            string fileName = $"{Guid.NewGuid()}.csv";
            string filePath = Path.Combine(directoryPath, fileName);
            string readFileName =  "test.csv";
            string readFilePath = Path.Combine(directoryPath, readFileName);

            List<string[]>data = new List<string[]>() 
            {
                new string[] {"John", "40", "CPA"},
                new string[] {"Cindy", "30", "software Engineering"},
                new string[] {"Bob", "28", "accountant"}
            };

            using (StreamWriter writer = new StreamWriter(filePath)) 
            {
                writer.WriteLine("Name, Age, Occupation");
                foreach(var item in data) 
                {
                    writer.WriteLine(string.Join(",",item));            
                }         
            }
            Console.WriteLine($"CSV file written successfully");


            using (StreamReader reader = new StreamReader(readFilePath)) 
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    Console.WriteLine(line);
                }
            }           
            Console.ReadKey();
        }
    }
}
