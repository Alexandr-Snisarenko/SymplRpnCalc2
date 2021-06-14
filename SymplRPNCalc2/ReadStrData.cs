using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymplRPNCalc2
{
    static class ReadStrData
    {
        public static string FromConsole(string promptMsg)
        {
            Console.Write(promptMsg);
            return Console.ReadLine();
        }
    }
}
