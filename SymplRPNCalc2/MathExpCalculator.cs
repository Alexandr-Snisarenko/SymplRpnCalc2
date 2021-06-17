using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymplRPNCalc2
{
    //базовый класс для калькуляторов тестовых выражений
    abstract class MathExpCalculator: IConsoleInOut
	{
		//конствнты
		protected const string cLegalChr = "1234567890+-*/()., "; //список допустимых символов
		protected const string cNumber = "1234567890"; //цифры
		protected const string cOper = "+-*/"; //операторы
		protected const string cPrnts = "()"; //скобки
		protected const string cOper1 = "+-";//операторы нижнего приоритета
		protected const string cOper2 = "*/";//операторы верхнего приоритета

		//атрибуты
		protected string mathExpr;
		protected double calcResult;

		public string MathExpr { get => mathExpr; }
		public double CalcResult { get => calcResult; }

		abstract public void CalcExpressionFromConsole();
		abstract public void WriteResultToConsole();

		abstract protected void Calc();

		public MathExpCalculator()
		{
			mathExpr = "";
			calcResult = 0;
		}

		public void CalcExpression(string mathExpression)
        {
			string errMsg;

			mathExpression = NormalizeMathExpression(mathExpression);
			
			if (CheckMathExpressionSyntax(mathExpression, out errMsg))
			{
				mathExpr = mathExpression;
			}
			else
				throw new Exception("Syntax Error: " + errMsg);

			Calc();
		}

		//нормализация математическго выражения убираем пробелы и меняем точку на запятую в дробной части
		static string NormalizeMathExpression(string mathExpression)
		{
			string normStr = "";

			//обход строки с мат выражением
			foreach (char curChr in mathExpression)
			{
				//если текущий символ пробел - переходим к следующему символу
				if (curChr == ' ')
					continue;

				//если текущий символ '.' - добавляем к нормализованной строке  ','
				if (curChr == '.')
					normStr += ',';
				else //иначе - добавляем текущий символ
					normStr += curChr;
			}

			return normStr;
		}

		//проверка общих правил построения математического выражения
		static bool CheckMathExpressionSyntax(in string mathExpr, out string errMsg)
		{
			int cntLeftLPrnts = 0; //кол-во левых скобок
			int cntRightPrnts = 0; //кол-во правых скобок
			char prevChr = '\0'; //переменная для предыдущего символа при анализе последовательности символов

			errMsg = "";
			try
			{

				//проверка первого символа. если оператор и это не '-'  - ошибка
				if (cOper.Contains(mathExpr[0]) && mathExpr[0] != '-')
					throw new Exception("First symbol is Operator.");

				//проверка крайнего символа. если оператор - ошибка
				if (cOper.Contains(mathExpr[mathExpr.Length - 1]))
					throw new Exception("Last symbol is Operator.");


				//обход строки выражения
				foreach (char curChr in mathExpr)
				{
					//проверяем на допустимые символы
					if (!cLegalChr.Contains(curChr))
						throw new Exception("Unexpected symbol:'" + curChr + "'");


					//если есть предыдущий символ - проверяем на допустимые комбинации символов
					if (prevChr != '\0')
					{
						// 2 оператора подряд
						if (cOper.Contains(curChr) && cOper.Contains(prevChr))
							throw new Exception("Double operators" + curChr + prevChr);

						//число перед левой скобкой 
						if (curChr == '(' && cNumber.Contains(prevChr))
							throw new Exception("Number before left parenthesis");

						//число после правой скобкой 
						if (cNumber.Contains(curChr) && prevChr == ')')
							throw new Exception("Number after right parenthesis");

						//оператор перед правой скобкой 
						if (curChr == ')' && cOper.Contains(prevChr))
							throw new Exception("Operator before right parenthesis");

						//оператор после левой скобкой и это не '-'
						if (cOper.Contains(curChr) && prevChr == '(' && curChr != '-')
							throw new Exception("Operator after left parenthesis");
					}

					//считаем скобки
					if (curChr == '(')
						cntLeftLPrnts++;
					if (curChr == ')')
						cntRightPrnts++;

					//если правых скобок больше чем левых - нарушение порядка скобок
					if (cntRightPrnts > cntLeftLPrnts)
						throw new Exception("Error of parentheses combinatios (right parenthesis before left parenthesis).");

					prevChr = curChr;
				}// main for

				//проверяем количество левых и правых скобок
				if (cntLeftLPrnts != cntRightPrnts)
					throw new Exception("Count of left parentheses not equal to count of rihgt parentheses.");

			}
			catch(Exception e)
            {
				errMsg = e.Message;
				return false;
            }

			return true;
		}


	}
}
