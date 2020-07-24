using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        /// ch是+或-
        /// </summary>
        /// <param name="ch">字符</param>
        /// <returns>是与否</returns>
        public static bool IsLowOperator(char ch) =>
            ch == '+' || ch == '-';

        /// <summary>
        /// ch是乘或除
        /// </summary>
        /// <param name="ch">字符</param>
        /// <returns>是与否</returns>
        public static bool IsUpOperator(char ch) =>
            ch == '*' || ch == '/';

        /// <summary>
        /// ch是运算符
        /// </summary>
        /// <param name="ch">字符</param>
        /// <returns>是与否</returns>
        public static bool IsOperator(char ch) =>
            IsLowOperator(ch) || IsUpOperator(ch);

        /// <summary>
        /// ch是数字
        /// </summary>
        /// <param name="ch">字符</param>
        /// <returns>是与否</returns>
        public static bool IsNumber(char ch) =>
            ch >= '0' && ch <= '9';

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
            while (str.Length > 0 && (IsNumber(str[0]) || (str[0] == '.' && !isDouble)))
            {
                if (str[0] == '.')
                    isDouble = true;
                result += str[0];
                str = str.Remove(0, 1);
                ++index;
            }
            return result;
        }
    }
}
