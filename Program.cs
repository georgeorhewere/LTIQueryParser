using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTIQueryParser
{
    class Program
    {
        static void Main(string[] args)
        {
            var sanitizer = new SanitizeFilter();
            sanitizer.Filter = $"name='Reg' AND description~'Math'";
            sanitizer.processFilter();
            Console.ReadLine();
        }
    }
}
