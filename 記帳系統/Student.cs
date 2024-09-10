using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 記帳系統
{
    internal class Student : IDisposable
    {
        public void Dispose()
        {
            Console.WriteLine("memory has been clear!!");
        }
    }
}
