namespace Task15
{
    class Program
    {
        static void Main()
        {
            int NumberOfDigits(string str)
            {
                int result = 0;
                for (int i=0; i < str.Length; i++)
                {
                    if (Char.IsDigit(str[i])) result++;
                }
                return result;
            }

            int NumberOfSpaces(string str)
            {
                int result = 0;
                for (int i = 0; i < str.Length; i++)
                {
                    if (str[i] == ' ') result++;
                }
                return result;
            }

            MyArrayDeque<string> myArrayDeque = new MyArrayDeque<string>();

            string inputFilePath = "/Users/michael/Documents/University (original)/2 course/casd/casd-labs/Task3/Task15/Task15/input.txt";
            string writeFilePath = "/Users/michael/Documents/University (original)/2 course/casd/casd-labs/Task3/Task15/Task15/sorted.txt";
            string? line;

            using (StreamReader reader = new StreamReader(inputFilePath))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    if (NumberOfSpaces(line) > 0)
                    {
                        if (!myArrayDeque.IsEmpty())
                        {
                            string first = myArrayDeque.GetFirst();
                            if (NumberOfDigits(first) < NumberOfDigits(line))
                            {
                                myArrayDeque.AddLast(line);
                            }
                            else
                            {
                                myArrayDeque.AddFirst(line);
                            }
                        }
                        else
                        {
                            myArrayDeque.Add(line);
                        }
                    }
                }
            }
            Console.Write("Введите количество пробелов, необходимых для того, чтобы удалить строку: ");
            int n = Convert.ToInt32(Console.ReadLine());
            using (StreamWriter writer = new StreamWriter(writeFilePath))
            {
                
                MyArrayDeque<string> myArrayDeque2 = new MyArrayDeque<string>(); 
                while (!myArrayDeque.IsEmpty())
                {
                    
                    line = myArrayDeque.RemoveFirst();
                    writer.WriteLine(line);
                    if(NumberOfSpaces(line) < n) myArrayDeque2.AddFirst(line);
                }
                myArrayDeque = myArrayDeque2;
            }
            Console.WriteLine("Количество элементов в двунаправленной очереди: " + myArrayDeque.Size());
        }
    }
}

