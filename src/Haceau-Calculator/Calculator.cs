using System;
using System.Collections.Generic;

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
        public dynamic Calculation()
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
            List<string> stack = new List<string>();
            List<bool> isOperNum = new List<bool>();
            List<char> oper = new List<char>();
            for (int index = 0; index < expression.Length; ++index)
            {
                if (expression[index] == '(')
                    stack.Add("(");
                else if (expression[index] == ')')
                {
                    while (isOperNum.ToArray().Length != 0)
                    {
                        postfix.Add(Tools.Pop(ref oper).ToString());
                        Tools.Pop(ref isOperNum);
                    }

                    while (stack.ToArray().Length > 0 && Tools.Top(stack) != "(")
                        postfix.Add(Tools.Pop(ref stack).ToString());

                    Tools.Pop(ref stack);
                }
                else if (Tools.IsLowOperator(expression.Substring(index)))
                {
                    if (postfix.ToArray().Length == 0 || expression[index - 1] == '(')
                    {
                        isOperNum.Add(true);
                        oper.Add(expression[index]);
                    }
                    else
                    {
                        while (stack.ToArray().Length > 0 && Tools.IsOperator(Tools.Top(stack).ToString()) >= 0)
                        {
                            index += Tools.IsOperator(Tools.Top(stack).ToString());
                            postfix.Add(Tools.Pop(ref stack).ToString());
                        }
                        stack.Add(expression[index].ToString());
                    }
                }
                else if (Tools.IsUpOperator(expression.Substring(index)) >= 0)
                {

                    while (stack.ToArray().Length > 0 && Tools.IsUpOperator(Tools.Top(stack).ToString()) > 0)
                    {
                        index += Tools.IsUpOperator(Tools.Top(stack).ToString());
                        postfix.Add(Tools.Pop(ref stack).ToString());
                    }
                    stack.Add(Tools.GetOperator(expression.Substring(index), out int addIndex));
                    index += addIndex;
                    index += Tools.IsUpOperator(expression.Substring(index));
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
                {
                    if (expression.Substring(index, 2) == "pi")
                    {
                        if (isOperNum.ToArray().Length - 1 >= 0)
                        {
                            Tools.Pop(ref isOperNum);
                            if (Tools.Pop(ref oper) == '+')
                                postfix.Add('+' + Math.PI.ToString());
                            else
                                postfix.Add('-' + Math.PI.ToString());
                            ++index;
                        }
                        else
                            postfix.Add(Math.PI.ToString());
                        ++index;
                        continue;
                    }
                    string funcName = Tools.GetFunction(expression.Substring(index), out int ind, out double val);
                    index += ind;

                    if (isOperNum.ToArray().Length - 1 >= 0)
                    {
                        Tools.Pop(ref isOperNum);
                        if (Tools.Pop(ref oper) == '+')
                            postfix.Add('+' + ReadFunction(funcName, val));
                        else
                            postfix.Add('-' + ReadFunction(funcName, val));
                        ++index;
                    }
                    postfix.Add(ReadFunction(funcName, val));
                }
            }
            while (stack.ToArray().Length != 0)
                postfix.Add(Tools.Pop(ref stack).ToString());
            Console.WriteLine();
        }

        /// <summary>
        /// 计算后缀表达式
        /// </summary>
        /// <returns>得数</returns>
        private dynamic CalculationPostfixExpression()
        {
            List<dynamic> stack = new List<dynamic>();
            for (int index = 0; index < postfix.ToArray().Length; ++index)
            {
                if (Tools.IsNumber(postfix[index].ToString()))
                    stack.Add((dynamic)double.Parse(postfix[index]));
                else if (stack.ToArray().Length < 2)
                {
                    dynamic num = Tools.Pop(ref stack);

                    if (postfix[index] == "+")
                        stack.Add(num);
                    else
                        stack.Add(-num);
                }
                else
                {
                    dynamic num2 = Tools.Pop(ref stack);
                    dynamic num1 = Tools.Pop(ref stack);

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
                    else if (postfix[index] == "^" || postfix[index] == "**")
                        stack.Add((dynamic)Math.Pow((double)num1, (double)num2));
                    else if (postfix[index] == "//")
                        stack.Add((int)(num1 / num2));
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
            if (Tools.IsOperator(expression[^1].ToString()) != -1)
                throw new Exception("不能以运算符结尾。");
            if (Tools.IsUpOperator(expression[0].ToString()) != -1)
                throw new Exception("不能以+-除外的运算符开头。");
            bool isOperator = false;
            bool upIsOperator = false;
            List<string> stack = new List<string>();
            for (int i = 0; i < expression.Length; ++i)
            {
                if (Tools.IsOperator(expression[i].ToString()) != -1)
                {
                    if (isOperator && (expression[i] == '*' || expression[i] == '/') && !upIsOperator)
                        upIsOperator = true;
                    else if (isOperator)
                        throw new Exception("运算符不能相邻。");
                    else
                        isOperator = true;
                }
                else
                    isOperator = false;
            }
        }

        private string ReadFunction(string funcName, double value)
        {
            switch (funcName)
            {
                case "abs":
                    return Math.Abs(value).ToString();
                    
                case "acos":
                    return Math.Acos(value).ToString();
                    
                case "acosh":
                    return Math.Acosh(value).ToString();
                    
                case "asin":
                    return Math.Asin(value).ToString();
                    
                case "asinh":
                    return Math.Asinh(value).ToString();
                    
                case "atan":
                    return Math.Atan(value).ToString();
                    
                case "atanh":
                    return Math.Atanh(value).ToString();
                    
                case "cbrt":
                    return Math.Cbrt(value).ToString();

                case "ceiling":
                    return Math.Ceiling(value).ToString();
                    
                case "cos":
                    return Math.Cos(value).ToString();
                    
                case "cosh":
                    return Math.Cosh(value).ToString();
                    
                case "exp":
                    return Math.Exp(value).ToString();
                    
                case "floor":
                    return Math.Floor(value).ToString();
                    
                case "ilogb":
                    return Math.ILogB(value).ToString();
                    
                case "log":
                    return Math.Log(value).ToString();
                    
                case "log10":
                    return Math.Log10(value).ToString();
                    
                case "log2":
                    return Math.Log2(value).ToString();
                    
                case "round":
                    return Math.Round(value).ToString();
                    
                case "sign":
                    return Math.Sign(value).ToString();
                    
                case "sin":
                    return Math.Sin(value).ToString();
                    
                case "sinh":
                    return Math.Sinh(value).ToString();
                    
                case "sqrt":
                    return Math.Sqrt(value).ToString();
                    
                case "tan":
                    return Math.Tan(value).ToString();
                    
                case "tanh":
                    return Math.Tanh(value).ToString();
                    
                case "truncate":
                    return Math.Truncate(value).ToString();
                    
                default:
                    throw new Exception("未知的字符。");
            }
        }
    }
}
