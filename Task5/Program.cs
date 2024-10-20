using System;
using System.IO;

namespace Task5
{
    class Program
    {
        static void Main()
        {
            string filePath = 
                "/Users/michael/Documents/University (original)/2 course/casd/casd-labs/Task3/Task5/Task5/input.txt";
            MyArrayList<string> DynoArray = new MyArrayList<string>([]);
            try
            {
                using (StreamReader sr = new StreamReader(filePath))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        line = line.Trim();
                        bool flag = false;
                        string str = "";
                        foreach (char character in line)
                        {
                            if (character == '<')
                            {
                                str = "" + character;
                                flag = true;
                            }
                            else if (character == '>' & flag)
                            {
                                str += character;
                                flag = false;
                                if (str[0] == '<' & str[str.Length-1] == '>')
                                    DynoArray.Add(str);
                            }
                            else if (flag)
                            {
                                str += character;
                            }
                            else
                            {
                                str = "";
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            for (int i = 0; i < DynoArray.Size(); i++)
            { 
                //"</HtMl>".ToLower();
                for (int j = i+1; j < DynoArray.Size(); j++)
                {
                    var tmp1 = DynoArray.Get(i).Replace("/","");
                    var tmp2 = DynoArray.Get(j).Replace("/","");
                    if (tmp1.ToLower() == tmp2.ToLower())
                    {
                        DynoArray.remove(j);
                    }
                }
            }

            int len = DynoArray.Size();
            for (int i = 0; i < len; i++)
            {
                Console.WriteLine(DynoArray.Get(i));
            }
        }
    }
}
