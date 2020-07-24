using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace Haceau.Application.Calculator
{
    public class Calculator
    {
        /// <summary>
        /// 表达式
        /// </summary>
        public string expression = "";
        /// <summary>
        /// 后缀表达式
        /// </summary>
        private List<string> postfix = new List<string>();

        /// <summary>
        /// 计算
        /// </summary>
        /// <returns>计算结果</returns>
        public decimal Calculation()
        {
            IsExpression();
            GetPostfixExpression();

            return CalculationPostfixExpression();
        }

        /// <summary>
        /// 获取后缀表达式
        /// </summary>
        private void GetPostfixExpression()
        {
            List<char> stack = new List<char>();
            List<bool> isOperNum = new List<bool>();
            List<char> oper = new List<char>();
            for (int index = 0; index < expression.Length; ++index)
            {
                if (expression[index] == '(')
                    stack.Add('(');
                else if (expression[index] == ')')
                {
                    while (isOperNum.ToArray().Length != 0)
                    {
                        postfix.Add(Tools.Pop(ref oper).ToString());
                        Tools.Pop(ref isOperNum);
                    }

                    while (stack.ToArray().Length > 0 && Tools.Top(stack) != '(')
                        postfix.Add(Tools.Pop(ref stack).ToString());

                    Tools.Pop(ref stack);
                }
                else if (Tools.IsLowOperator(expression[index]))
                {
                    if (postfix.ToArray().Length == 0 || expression[index - 1] == '(')
                    {
                        isOperNum.Add(true);
                        oper.Add(expression[index]);
                    }
                    else
                    {
                        while (stack.ToArray().Length > 0 && Tools.IsOperator(Tools.Top(stack)))
                            postfix.Add(Tools.Pop(ref stack).ToString());
                        stack.Add(expression[index]);
                    }
                }
                else if (Tools.IsUpOperator(expression[index]))
                {
                    while (stack.ToArray().Length > 0 && Tools.IsUpOperator(Tools.Top(stack)))
                        postfix.Add(Tools.Pop(ref stack).ToString());
                    stack.Add(expression[index]);
                }
                else if (Tools.IsNumber(expression[index].ToString()))
                {
                    int addIndex;
                    if (isOperNum.ToArray().Length - 1 >= 0)
                    {
                        Tools.Pop(ref isOperNum);
                        if (Tools.Pop(ref oper) == '+')
                            postfix.Add('+' + Tools.GetDouble(expression.Substring(index), out addIndex));
                        else
                            postfix.Add('-' + Tools.GetDouble(expression.Substring(index), out addIndex));
                    }
                    else
                        postfix.Add(Tools.GetDouble(expression.Substring(index), out addIndex));

                    index += addIndex;
                }
                else
                    throw new Exception("未知的字符。");
            }
            while (stack.ToArray().Length != 0)
                postfix.Add(Tools.Pop(ref stack).ToString());
            Console.WriteLine();
        }

        /// <summary>
        /// 计算后缀表达式
        /// </summary>
        /// <returns>得数</returns>
        private decimal CalculationPostfixExpression()
        {
            List<decimal> stack = new List<decimal>();
            for (int index = 0; index < postfix.ToArray().Length; ++index)
            {
                if (Tools.IsNumber(postfix[index].ToString()))
                    stack.Add(decimal.Parse(postfix[index]));
                else if (stack.ToArray().Length < 2)
                {
                    decimal num = Tools.Pop(ref stack);

                    if (postfix[index] == "+")
                        stack.Add(num);
                    else
                        stack.Add(-num);
                }
                else
                {
                    decimal num2 = Tools.Pop(ref stack);
                    decimal num1 = Tools.Pop(ref stack);

                    if (postfix[index] == "+")
                        stack.Add(num1 + num2);
                    else if (postfix[index] == "-")
                        stack.Add(num1 - num2);
                    else if (postfix[index] == "*")
                        stack.Add(num1 * num2);
                    else if (postfix[index] == "/")
                        stack.Add(num1 / num2);
                    else if (postfix[index] == "%")
                        stack.Add(num1 % num2);
                    else
                        throw new Exception("未知的运算符。");
                }
            }
            return Tools.Pop(ref stack);
        }

        /// <summary>
        /// 输入为表达式
        /// </summary>
        private void IsExpression()
        {
            if (Tools.IsOperator(expression[^1]))
                throw new Exception("不能以运算符结尾。");
            bool isOperator = false;
            for (int i = 0; i < expression.Length; ++i)
            {
                if (Tools.IsOperator(expression[i]))
                {
                    if (isOperator)
                        throw new Exception("运算符不能相邻。");
                    else
                        isOperator = true;
                }
                else
                    isOperator = false;
            }
        }
    }
}
