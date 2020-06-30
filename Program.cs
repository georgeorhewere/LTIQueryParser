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
            sanitizer.Filter = $"name='Bingo Blitz'ANDdescription~'Math'";
            sanitizer.processFilter();
            Console.ReadLine();
        }
    }
}
