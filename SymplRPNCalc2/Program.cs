using System;

namespace SymplRPNCalc2
{

    class Program
    {
        static void Main(string[] args)
        {
            MathRPNCalculator mCalc = new MathRPNCalculator();
            
            do
            {
                mCalc.CalcExpressionFromConsole();

                Console.WriteLine("For exit press 'Esc'.For continue press anykey.");

            } while (Console.ReadKey().Key != ConsoleKey.Escape);

        }
    }
}
