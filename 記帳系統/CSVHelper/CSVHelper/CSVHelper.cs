using Microsoft.Analytics.Interfaces;
using Microsoft.Analytics.Interfaces.Streaming;
using Microsoft.Analytics.Types.Sql;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CSVtransform
{
    public class CSVHelper
    {
        public static void WriteCSV(string filePath, string combinedData) 
        {
            using (StreamWriter writer = new StreamWriter(filePath,false, Encoding.UTF8)) 
            {
                writer.WriteLine(combinedData);
            }
        }

        public void ReadCSV(string filePath)
        {          
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string line = reader.ReadLine();
                    Console.WriteLine(line);
                    if (line != null)
                    {
                        string[] fields = line.Split(',');
        }
    }
