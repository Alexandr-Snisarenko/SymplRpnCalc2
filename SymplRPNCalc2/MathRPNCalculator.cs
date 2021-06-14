using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymplRPNCalc2
{
	class MathRPNCalculator : MathExpCalculator
	{
		string rpnExpr;
		Queue <string> rpnQu;
        public string RpnExpr { get => rpnExpr; }
        public MathRPNCalculator() : base()
		{
			rpnQu = new();
			rpnExpr = "";
		}

		protected override void Calc()
		{
			string errMsg = "";

			if (!ConvertMathExpressionToRPN(mathExpr, out rpnExpr, ref rpnQu, out errMsg))
				throw new Exception("Convertation Error: " + errMsg);

			CalcRPNQueue();
		}

		public static bool ConvertMathExpressionToRPN(string mathExpr, out string rpnExpr, ref Queue<string> rpnQu, out string errMsg)
        {
			bool res = true;
			string numStr = ""; //строка для формирования отдельных операндов из входной строки с мат. выражением
			Stack<char> depoStk = new(); //рабочий "депо" стек. для временного хранения операторов
			char curChr = '\0'; //операционная перемнная для обхода строки 

			rpnExpr = "";
			errMsg = "";
			rpnQu.Clear();

			try
			{
				//обход строки с мат выражением
				for (int i = 0; i < mathExpr.Length; i++)
				{
					curChr = mathExpr[i];

                    /////Унарные операторы
                    //Если первый символ '-' и дальше идет число, добавляем его в токен
                    if(i == 0 && curChr == '-' && cNumber.Contains(mathExpr[1]))
                    {
						numStr += curChr;
						continue;
					}
					//Если текущий символ '-' и он не последний и перед ним был '(', а после него - цифра -  добавляем его в токен 
					if ((curChr == '-') && (i != mathExpr.Length-1) && (mathExpr[i-1] == '(') && cNumber.Contains(mathExpr[i+1]))
					{
						numStr += curChr;
						continue;
					}

					//если текущий символ - число или запятая - добавляем его к строке numStr
					if (cNumber.Contains(curChr) || curChr == ',')
                    {
						numStr += curChr;
					}
					else  // если текущий сивол не число, то, если строка numStr не пустая, переносим её в rpn очередь и очищаем строку numStr
						if (numStr.Length > 0)
					{
						rpnQu.Enqueue(numStr);
						numStr = "";
					}

					//если текущий символ - левая скобка - в стек "депо"
					if (curChr == '(')
						depoStk.Push(curChr);

					//если текущий символ - правая скобка 
					if (curChr == ')')
					{
						//то пока не встретиться левая скобка переносим операторы из депо стека в rpn очередь 
						while (depoStk.Peek() != '(')
						{
							rpnQu.Enqueue(depoStk.Pop().ToString());

							if (depoStk.Count == 0)
								throw new Exception("End of Mathstring without left parentsis");

						}

						//удаляем левую скобку из депо стека
						depoStk.Pop();
					}

					//если текущий символ - оператор 
					if (cOper.Contains(curChr))
					{
						//если депо стек не пустой и крайний элемент - опреатор
						if (depoStk.Count > 0 && cOper.Contains(depoStk.Peek()))
						{
							//то, если текущий оператор нижнего приоритета или 
							// (текущий оператор верхнего приоритета и в стеке крайний элемент - оператор верхнего приоритета), то
							// переносим крайний элемент (оператор) из депо стека в rpn очередь
							if (cOper1.Contains(curChr) ||
								(cOper2.Contains(curChr) && cOper2.Contains(depoStk.Peek())))
								rpnQu.Enqueue(depoStk.Pop().ToString());
						}

						//текущий оператор помещаем в рабочий стек
						depoStk.Push(curChr);
					}


				}

				//переносим в RPN очередь крайнее число исходной строки (если крайняя скобка - то будет пустым)
				if (numStr.Length > 0)
					rpnQu.Enqueue(numStr);

				//переносим оставшиеся операторы из "депо" в RPN очередь
				while (depoStk.Count > 0)
					rpnQu.Enqueue(depoStk.Pop().ToString());
				
				//Формируем строку RPN выражения
				foreach (string s in rpnQu)
					rpnExpr += s + ' ';
			}
			catch (Exception e)
            {
				res = false;
				errMsg = e.Message;
            }

			return res;
        }
		
        void CalcRPNQueue()
        {
			double res = 0; //для результов промежуточных вычислений
			double n1, n2; //для промежуточных значений операндов при вычислении операций
			string curTok = ""; //текущий токен
			Stack<string> expStk = new(); //рабочий стек для обработки выражения

			if (rpnQu.Count == 0)
				throw new Exception("Expression is empty");


			//віполняем обработку очереди, которая содержит выражением в порядке RPN 
			while (rpnQu.Count > 0)
			{
				//берем токен из очереди
				curTok = rpnQu.Dequeue();

				//если токен - не операция - помещаем его в рабочий стек (т.о. в стеке будут только числа)
				if (curTok.Length > 1 || !cOper.Contains(curTok[0]))
				{
					expStk.Push(curTok);
				}
				else //иначе, если текущий токен - операция, то выполняем текущую операцию над двума крайними значениями из рабочего стека
				{
					//если в стеке меньше двух элементов - что то пошло не так
					if (expStk.Count < 2)
						throw new Exception("Expression error.");

					//получаем элементы из стека и переводим их в дабл
					n2 = Convert.ToDouble(expStk.Pop());
					n1 = Convert.ToDouble(expStk.Pop());
		
					//выполнение операций в зависимости от значения текущего оператора
					switch (curTok[0])
					{
						case '+':
							res = n1 + n2;
							expStk.Push(Convert.ToString(res));
							break;
						case '-':
							res = n1 - n2;
							expStk.Push(Convert.ToString(res));
							break;
						case '*':
							res = n1 * n2;
							expStk.Push(Convert.ToString(res));
							break;
						case '/':
							if (n2 == 0)
								throw new Exception("Division on zero");
							res = n1 / n2;
							expStk.Push(Convert.ToString(res));
							break;
						default:
							throw new Exception("Unexpected operator.");
						}

				}
			} //while

			//Получаем результат. Должен быть в стеке в первом (нулевом) и единственном элементе стека. Если это не так - ошибка.
			if (expStk.Count != 1)
				throw new Exception("Expression error(operStack not empty)");
			else
			{
				calcResult = Convert.ToDouble(expStk.Pop());
			}

        }

	}

}
