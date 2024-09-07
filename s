using System;
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
    public static ComplexNum operator *(ComplexNum a, ComplexNum b)
        => new ComplexNum(a.REAL * b.REAL - a.IMAGINARY * b.IMAGINARY, a.REAL * b.IMAGINARY + a.IMAGINARY * b.REAL);
    public static ComplexNum operator /(ComplexNum a, ComplexNum b)
        => new ComplexNum((a.REAL * b.REAL + a.IMAGINARY * b.IMAGINARY) / (b.REAL * b.REAL + b.IMAGINARY * b.IMAGINARY), (b.REAL * a.IMAGINARY - a.REAL * b.IMAGINARY) / (b.REAL * b.REAL - b.IMAGINARY * b.IMAGINARY));
    public static double Modulo(ComplexNum num) => Math.Sqrt(num.REAL*num.REAL+num.IMAGINARY*num.IMAGINARY);
    
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
        static void Main(string[] args)
        {
            
        }
    }
}
