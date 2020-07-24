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
            GetPostfixExpression();

            return CalculationPostfixExpression();
        }

        /// <summary>
        /// 获取后缀表达式
        /// </summary>
        private void GetPostfixExpression()
        {
            List<char> stack = new List<char>();
            for (int index = 0; index < expression.Length; ++index)
            {
                if (expression[index] == '(')
                    stack.Add('(');
                else if (expression[index] == ')')
                {
                    while (stack.ToArray().Length > 0 && Tools.Top(stack) != '(')
                        postfix.Add(Tools.Pop(ref stack).ToString());

                    Tools.Pop(ref stack);
                }
                else if (Tools.IsLowOperator(expression[index]))
                {
                    while (stack.ToArray().Length > 0 && Tools.IsOperator(Tools.Top(stack)))
                        postfix.Add(Tools.Pop(ref stack).ToString());
                    stack.Add(expression[index]);
                }
                else if (Tools.IsUpOperator(expression[index]))
                {
                    while (stack.ToArray().Length > 0 && Tools.IsUpOperator(Tools.Top(stack)))
                        postfix.Add(Tools.Pop(ref stack).ToString());
                    stack.Add(expression[index]);
                }
                else if (Tools.IsNumber(expression[index]))
                {
                    postfix.Add(Tools.GetDouble(expression.Substring(index), out int addIndex));
                    
                    index += addIndex;
                }
                else
                    throw new Exception("未知的字符。");
            }
            while (stack.ToArray().Length != 0)
                postfix.Add(Tools.Pop(ref stack).ToString());
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
                if (Tools.IsNumber(postfix[index][0]))
                    stack.Add(decimal.Parse(postfix[index]));
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
                    else
                        throw new Exception("未知的运算符。");
                }
            }
            return Tools.Pop(ref stack);
        }
    }
}
