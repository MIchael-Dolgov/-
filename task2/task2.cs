using System;
using System.Globalization;
//using System.Math;
public struct ComplexNum
{
    public ComplexNum(double real, double imaginary)
    {
        REAL = real;
        IMAGINARY = imaginary;
    }
    public double REAL;
    public double IMAGINARY;

    public override string ToString() => $"({REAL}, {IMAGINARY})";
    // static - принадлежит самой структуре
    public static ComplexNum operator +(ComplexNum a, ComplexNum b)
        => new ComplexNum(a.REAL + b.REAL, a.IMAGINARY + b.IMAGINARY);
    public static ComplexNum operator -(ComplexNum a, ComplexNum b)
        => new ComplexNum(a.REAL - b.REAL, a.IMAGINARY - b.IMAGINARY);
    public static ComplexNum operator *(ComplexNum a, ComplexNum b)
        => new ComplexNum(a.REAL * b.REAL - a.IMAGINARY * b.IMAGINARY, a.REAL * b.IMAGINARY + a.IMAGINARY * b.REAL);
    public static ComplexNum operator /(ComplexNum a, ComplexNum b)
        => new ComplexNum((a.REAL * b.REAL + a.IMAGINARY * b.IMAGINARY) / (b.REAL * b.REAL + b.IMAGINARY * b.IMAGINARY), (b.REAL * a.IMAGINARY - a.REAL * b.IMAGINARY) / (b.REAL * b.REAL - b.IMAGINARY * b.IMAGINARY));
    public static double Modulo(ComplexNum num) => Math.Sqrt(num.REAL * num.REAL + num.IMAGINARY * num.IMAGINARY);

    public static double ComplexArgument(ComplexNum num)
    {
        if (num.REAL > 0 & num.IMAGINARY >= 0)
            return Math.Atan(num.IMAGINARY / num.REAL);
        else if (num.REAL < 0 & num.IMAGINARY >= 0)
            return Math.PI - Math.Atan(num.IMAGINARY / num.REAL);
        else if (num.REAL < 0 & num.IMAGINARY < 0)
            return Math.PI + Math.Atan(num.IMAGINARY / num.REAL);
        else if (num.REAL > 0 & num.IMAGINARY < 0)
            return 2 * Math.PI - Math.Atan(num.IMAGINARY / num.REAL);
        else if (num.REAL == 0 & num.IMAGINARY > 0)
            return Math.PI / 2;
        else if (num.REAL == 0 & num.IMAGINARY < 0)
            return 3 * Math.PI / 2;
        else
            return 3 * Math.PI / 2;
    }

    public double real() => REAL;
    public double imaginary() => IMAGINARY;
}

namespace ConsoleApp
{
    class TestClass
    {
        static void Main()
        {
            bool appIsRunning = true;
            ComplexNum cmpNum = new ComplexNum(0, 0);
            while (appIsRunning)
            {
                Console.WriteLine("=====Menu=====");
                Console.WriteLine("Текущее комплексное число: " + cmpNum.ToString());
                Console.WriteLine("1) Ввести комплексное число");
                Console.WriteLine("2)Сложить комплексные числа");
                Console.WriteLine("3)Вычесть комплексные числа");
                Console.WriteLine("4)Умножить комплексные числа");
                Console.WriteLine("5)Поделить комплексные числа");
                Console.WriteLine("6)Найти модуль введённого комплексного числа");
                Console.WriteLine("7)Найти аргумент введённого комплексного числа");
                Console.WriteLine("8)Вернуть мнимую часть числа");
                Console.WriteLine("9)Вернуть вещественную часть числа");
                Console.WriteLine("Q/q)Выход");
                int n;

                string[] textVar;
                switch (Console.ReadLine())
                {
                    case "1":
                        Console.Clear();
                        Console.Write("Введите число: ");
                        textVar = Console.ReadLine().Trim().Split();
                        cmpNum.REAL = int.Parse(textVar[0]);
                        cmpNum.IMAGINARY = int.Parse(textVar[1]);
                        break;
                    case "2":
                        Console.Clear();
                        Console.Write("Введите второе число: ");
                        textVar = Console.ReadLine().Trim().Split();
                        {
                            ComplexNum cmpNum2 = new ComplexNum(int.Parse(textVar[0]), int.Parse(textVar[1]));
                            cmpNum += cmpNum2;
                        }
                        Console.WriteLine("Новое значение комлексного числа: " + cmpNum.ToString());
                        break;
                    case "3":
                        Console.Clear();
                        Console.Write("Введите второе число: ");
                        textVar = Console.ReadLine().Trim().Split();
                        {
                            ComplexNum cmpNum2 = new ComplexNum(int.Parse(textVar[0]), int.Parse(textVar[1]));
                            cmpNum -= cmpNum2;
                        }
                        Console.WriteLine("Новое значение комлексного числа: " + cmpNum.ToString());
                        break;
                    case "4":
                        Console.Clear();
                        Console.Write("Введите второе число, разделив мнимую и реальную части пробелом: ");
                        textVar = Console.ReadLine().Trim().Split();
                        {
                            ComplexNum cmpNum2 = new ComplexNum(int.Parse(textVar[0]), int.Parse(textVar[1]));
                            cmpNum *= cmpNum2;
                        }
                        Console.WriteLine("Новое значение комлексного числа: " + cmpNum.ToString());
                        break;
                    case "5":
                        Console.Clear();
                        Console.Write("Введите второе число, разделив мнимую и реальную части пробелом: ");
                        textVar = Console.ReadLine().Trim().Split();
                        {
                            ComplexNum cmpNum2 = new ComplexNum(int.Parse(textVar[0]), int.Parse(textVar[1]));
                            cmpNum /= cmpNum2;
                        }
                        Console.WriteLine("Новое значение комлексного числа: " + cmpNum.ToString());
                        break;
                    case "6":
                        Console.Write("Значение модуля текущего комплексного числа: " + ComplexNum.Modulo(cmpNum));
                        break;
                    case "7":
                        Console.WriteLine("Значение аргумента текущего комплексного числа: " + ComplexNum.ComplexArgument(cmpNum));
                        break;
                    case "8":
                        Console.WriteLine("Мнимая часть числа: " + cmpNum.IMAGINARY);
                        break;
                    case "9":
                        Console.WriteLine("Реальная часть числа: " + cmpNum.REAL);
                        break;
                    case "Q":
                    case "q":
                        Console.WriteLine("Quiting...");
                        appIsRunning = false;
                        break;
                    default:
                        Console.WriteLine("Неизвестная команда");
                        break;
                }
            }
        }
    }
}
