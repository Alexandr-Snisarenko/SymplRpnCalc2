using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymplRPNCalc2
{
    static class ShowStrData
    {
        public static void ToConsole(string strMsg, bool ClearConsole = false)
        {
            if (ClearConsole)
                Console.Clear();
            Console.WriteLine(strMsg);

        }
    }
}
