using System;

namespace SymplRPNCalc2
{
    
    class Program
    {
        static void Main(string[] args)
        {
            string str1, errMsg;
            MathRPNCalculator mCalc = new MathRPNCalculator();


            do
            {
                try
                {
                   ShowStrData.ToConsole("This application evaluates an arbitrary mathematical expression using reverse Polish notation.\n" +
                                           "The original expression can only contain: numbers, simple operations(+-*/ ) and parentheses.\n" +
                                           "For example: 4-2*(10-3*2)+15-7\n", true);

                    str1 = ReadStrData.FromConsole("Input math expression: ");
                    mCalc.CalcExpression(str1);

                    ShowStrData.ToConsole("\nInputed Expression: " + mCalc.MathExpr);
                    ShowStrData.ToConsole("RPN Expression: " + mCalc.RpnExpr);
                    ShowStrData.ToConsole("Resoult Expression: " + mCalc.CalcResult+'\n');
                }
                catch (Exception e)
                {
                    ShowStrData.ToConsole(e.Message);
                }

                ShowStrData.ToConsole("For exit press 'Esc'.For continue press anykey.");

            } while (Console.ReadKey().Key != ConsoleKey.Escape);



        }
    }
}
