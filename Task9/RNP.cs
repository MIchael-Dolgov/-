using System.Text.RegularExpressions;

namespace Task9;

public class RNP
{
    private string expression;
    private Queue<string> postfix = new Queue<string>();
    private double? value = null;
    
    public RNP(string expr)
    {
        expression = expr.Trim() + " ";
        CreateRNP();
    }

    public string Expression
    {
        get { return expression; }
    }

    public double? Value
    {
        get
        {
            if (value == null)
            {
                CalculateRNP();
            }
            return value;
        }
    }

    private delegate double BinaryOperation(double num1, double num2);

    private delegate double UnaryOperation(double num1);

    private static double Sum(double num1, double num2) => num1 + num2;
    private static double Sub(double num1, double num2) => num1 - num2;
    private static double Multp(double num1, double num2) => num1 * num2;
    private static double Div(double num1, double num2) => num1 / num2;
    private static double Minimum(double num1, double num2) => Math.Min(num1, num2);
    private static double Maximum(double num1, double num2) => Math.Max(num1, num2);
    private static double Remainder(double num1, double num2) => Math.Abs(num1) % Math.Abs(num2);

    private static readonly Dictionary<string, (int index, BinaryOperation? func)> BinaryOperations =
        new Dictionary<string, (int index, BinaryOperation func)>
        {
            { "+", (0, Sum) },
            { "-", (0, Sub) },
            { "*", (1, Multp) },
            { "/", (1, Div) },
            { "^", (2, Math.Pow) },
            { "min", (3, Minimum) },
            { "max", (3, Maximum) },
            { "%", (3, Remainder) },
            { "(", (-1, null)},
            { ")", (-1, null)}
        };
    
    private static readonly Dictionary<string, (int index, UnaryOperation func)> UnaryOperations =
        new Dictionary<string, (int index, UnaryOperation func)>
        {
            { "sin", (5, Math.Sin) },
            { "cos", (5, Math.Cos) },
            { "tg", (5, Math.Tanh) },
            { "ln", (5, Math.Log) },
            { "log", (5, Math.Log10) },
            { "sqrt", (5, Math.Sqrt) },
            { "abs", (5, Math.Abs) },
            { "exp", (5, Math.Exp) },
            { "trunc", (5, Math.Truncate) }
        };

    private void CreateRNP()
    {
        MyStack<string> stack = new MyStack<string>();
    
        string word = "";
        for (int i = 0; i < expression.Length; i++)
        {
            if (expression[i] == ' ')
            {
                if (word.Length == 0) continue;

                if (Regex.IsMatch(word, "^[a-z]$")) // Измените на [a-zA-Z] для переменных
                {
                    Console.WriteLine("Введите, чему численно равна переменная " + word + ": ");
                    word = Console.ReadLine().Trim();
                    postfix.Enqueue(word);
                }
                else if (Regex.IsMatch(word, @"^[-+]?[0-9]*\.?[0-9]+$"))
                {
                    postfix.Enqueue(word);
                }
                else if (UnaryOperations.ContainsKey(word))
                {
                    stack.Push(word);
                }
                else if (word == "(")
                {
                    stack.Push(word);
                }
                else if (word == ")")
                {
                    while (stack.Peek() != "(")
                    {
                        postfix.Enqueue(stack.Pop());
                    }
                    stack.Pop();
                }
                else if (BinaryOperations.ContainsKey(word))
                {
                    if (stack.Empty())
                    {
                        stack.Push(word);
                    }
                    else if (BinaryOperations.ContainsKey(stack.Peek()) && BinaryOperations[word].Item1
                             <= BinaryOperations[stack.Peek()].Item1)
                    {
                        postfix.Enqueue(stack.Pop());
                        stack.Push(word);
                    }
                    else
                    {
                        stack.Push(word);
                    }
                }
                word = "";
            }
            else
            {
                word += expression[i];
            }
        }
    
        while (!stack.Empty())
        {
            postfix.Enqueue(stack.Pop());
        }
    
        /*
        foreach (var item in postfix.ToArray())
        {
            Console.Write(item);
        }
        */
    }


    private void CalculateRNP()
    {
        MyStack<double> stack = new MyStack<double>();
        while (postfix.Count > 0)
        {
            var variable = postfix.Dequeue();
            if (UnaryOperations.ContainsKey(variable))
            {
                stack.Push(UnaryOperations[variable].Item2(Convert.ToDouble(stack.Pop())));
            }
            else if (BinaryOperations.ContainsKey(variable))
            {
                double right = stack.Pop();
                double left = stack.Pop();
                stack.Push(BinaryOperations[variable].Item2(left, right));
            }
            else
            {
                stack.Push(Convert.ToDouble(variable));
            }
        }
        value = stack.Pop();
    }
}
