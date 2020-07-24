using System;

namespace Haceau.Application.Calculator
{
    class Start
    {
        private static void Main()
        {
            while (true)
            {
                Console.WriteLine("Haceau Calculator v1.0.0");
                Console.WriteLine("输入表达式进行计算。");
                Calculator calculator = new Calculator();
                Tools.PromptInput("Expression");
                calculator.expression = Console.ReadLine().ToLower().Replace(" ", "");
                if (calculator.expression == "quit")
                    return;
                try
                {
                    Console.WriteLine($"运算结果： {calculator.Calculation()}");
                    Console.Write("请按任意键继续...");
                    Console.ReadKey();
                    Console.Clear();
                }
                catch (Exception e)
                {
                    Console.WriteLine("发生了错误！");
                    Console.WriteLine($"错误信息：{e.Message}");
                    Console.Write("请按任意键继续...");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
        }
    }
}
