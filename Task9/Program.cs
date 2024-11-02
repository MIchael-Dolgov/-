using System;
using System.Reflection.Metadata.Ecma335;

namespace Task9
{
    class Program
    {
        static void Main()
        {
            string? expression;
            Console.WriteLine("Введите ваше математическое выражение: ");
            expression = Console.ReadLine();
            if (expression == null) return;
            RNP polExpression = new RNP(expression);
            Console.WriteLine(polExpression.Value);
        }
    }
}

