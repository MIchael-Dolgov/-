using System;
using System.IO;

namespace Task7
{
    class Program
    {
        static void Main()
        {
            const string INPUT_FILE_PATH = "/Users/michael/Documents/University (original)/2 course/casd/casd-labs/Task3/Task7/Task7/input.txt";
            const string OUTPUT_FILE_PATH = "/Users/michael/Documents/University (original)/2 course/casd/casd-labs/Task3/Task7/Task7/output.txt";
            
            MyVector<string> IPs = new MyVector<string>();
            
            foreach (string line in File.ReadLines(INPUT_FILE_PATH))
            {
                string IP = "";
                int DigitTo3Cnt = 0;
                int DotTo3Cnt = 0;
                int BlocksCnt = 0;
                //IP4 check
                foreach (char ch in line.Trim().Replace(" ", "")+" ") //" " для добавления IP4 с края
                {
                    if (char.IsDigit(ch) & BlocksCnt <= 3)
                    {
                        if (DigitTo3Cnt < 3)
                        {
                            IP += ch;
                            DigitTo3Cnt++;
                        }
                        else
                        {
                            if (DotTo3Cnt != 3)
                            {
                                IP = IP.Substring(1) + ch;
                                //DigitTo3Cnt = 1;
                            }
                        }
                    }
                    else if (IP.Length >= 1 && IP[IP.Length-1] != ch && ch == '.' && DigitTo3Cnt <= 3)
                    {
                        if (DotTo3Cnt < 3)
                        {
                            IP += ch;
                            DigitTo3Cnt = 0;
                            DotTo3Cnt++;
                            BlocksCnt++;
                        }
                        else
                        {
                            if (BlocksCnt <= 3 & IP.Length != 0)
                            {
                                IPs.Add(IP);
                            }
                            IP = "";
                            DigitTo3Cnt = 0;
                            DotTo3Cnt = 0;
                            BlocksCnt = 0;
                        }
                    }
                    else
                    {
                        if (DigitTo3Cnt >= 1 && BlocksCnt == 3 & IP.Length != 0)
                        {
                            IPs.Add(IP);
                            IP = "";
                            DigitTo3Cnt = 0;
                            DotTo3Cnt = 0;
                            BlocksCnt = 0;
                        }
                        else
                        {
                            IP = "";
                            DigitTo3Cnt = 0;
                            DotTo3Cnt = 0;
                            BlocksCnt = 0; 
                        }
                    }
                }
            }

            /*
            for (int i = 0; i < IPs.Size(); i++)
            {
                for (int j = i + 1; j < IPs.Size(); j++)
                {
                    var tmp1 = IPs.Get(i).Split(".");
                    var tmp2 = IPs.Get(j).Split(".");
                    foreach (var part1 in tmp1)
                    {
                        foreach (var part2 in tmp2)
                        {
                            if (part1 == part2)
                            {
                                IPs.Remove(j);
                                break;
                            }
                        }
                    }
                }
            }
            */ 
            using (StreamWriter writer = new StreamWriter(OUTPUT_FILE_PATH))
            {
                for (int i = 0; i < IPs.Size(); i++)
                    writer.WriteLine(IPs.Get(i));
            }
        }
    }
}