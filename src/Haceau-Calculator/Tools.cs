using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Haceau.Application.Calculator
{
    public static class Tools
    {
        /// <summary>
        /// 打印输入提示
        /// </summary>
        /// <param name="message">输入提示的内容</param>
        public static void PromptInput(string message) =>
            Console.Write($"{message} > ");

        /// <summary>
        /// 读取用户输入的整数，如果输入的不是整数，提示输入错误并打印输入提示
        /// </summary>
        /// <param name="message">输入提示的内容</param>
        /// <returns>输入的整数</returns>
        public static int ReadInt(string message)
        {
            int result = 0;
            while (int.TryParse(Console.ReadLine(), out result))
            {
                Console.WriteLine("输入错误！");
                Console.Write($"{message} > ");
            }
            return result;
        }

        /// <summary>
        /// 弹出栈顶
        /// </summary>
        /// <typeparam name="T">栈存储的数据类型</typeparam>
        /// <param name="stack">栈</param>
        /// <returns>获取的值</returns>
        public static T Pop<T>(ref List<T> stack)
        {
            T result = Top(stack);
            stack.RemoveAt(stack.ToArray().Length - 1);
            return result;
        }

        /// <summary>
        /// 获取栈顶值
        /// </summary>
        /// <typeparam name="T">栈存储的数据类型</typeparam>
        /// <param name="stack">栈</param>
        /// <returns>获取的值</returns>
        public static T Top<T>(List<T> stack) =>
            stack[stack.ToArray().Length - 1];

        /// <summary>
        /// str是+或-
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>发生了变化的index长度</returns>
        public static bool IsLowOperator(string str) =>
            str[0] == '+' || str[0] == '-';

        /// <summary>
        /// str是*或^或**或/或//或%
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>发生了变化的index长度</returns>
        public static int IsUpOperator(string str) =>
            (str.Length > 1 && ((str[0] == '*' && str[1] == '*') || (str[0] == '*' && str[1] == '*'))) ? 1 : ((str[0] == '*' || str[0] == '/' || str[0] == '%' || str[0] == '^') ? 0 : -1);

        /// <summary>
        /// str是运算符
        /// </summary>
        /// <param name="str">字符</param>
        /// <returns>发生了变化的index长度</returns>
        public static int IsOperator(string str) =>
            IsUpOperator(str) == -1 ? (IsLowOperator(str) ? 0 : -1) : IsUpOperator(str);

        /// <summary>
        /// str是数字
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>是与否</returns>
        public static bool IsNumber(string str) =>
            (str[0] >= '0' && str[0] <= '9') || (str.Length - 1 > 0 && (str[0] == '+' || str[0] == '-') && str[1] >= '0' && str[1] <= '9');


        /// <summary>
        /// 截取str中最开始的运算符，遇到非运算符字符结束。
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="index">影响index数</param>
        /// <returns>运算符</returns>
        public static string GetOperator(string str, out int index)
        {
            string result = "";
            index = -1;
            char? oper = null;
            while (str.Length > 0 && (IsUpOperator(str) != -1) && (oper == null || str[0] == oper))
            {
                result += str[0];
                oper = str[0];
                str = str.Remove(0, 1);
                ++index;
            }
            return result;
        }

        /// <summary>
        /// 截取str中最开始的数字，遇到非数字字符结束
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>数字字符串</returns>
        public static string GetDouble(string str, out int index)
        {
            string result = "";
            bool isDouble = false;
            index = -1;
            while (str.Length > 0 && (IsNumber(str[0].ToString()) || (str[0] == '.' && !isDouble)))
            {
                if (str[0] == '.')
                    isDouble = true;
                result += str[0];
                str = str.Remove(0, 1);
                ++index;
            }
            return result;
        }

        /// <summary>
        /// 读取函数
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static string GetFunction(string str, out int index, out double value)
        {
            int num = 0;
            int start = str.IndexOf('(');
            int end = str.IndexOf(')');
            string sstr = str.Substring(start + 1, end - start - 1);
            for (int i = 0; i < sstr.Length; ++i)
                if (sstr[i] == '(')
                    ++num;
            while (num != 0)
            {
                end = str.IndexOf(')', end + 1);
                --num;
            }
            sstr = str.Substring(start + 1, end - start - 1);
            index = end;
            string result = str.Substring(0, start);
            value = new Calculator(){ expression = sstr }.Calculation();
            return result;
        }
    }
}