using System;
using System.Diagnostics;

namespace Task22.Models
{
    public class RandomStringGenerator
    {
        private static readonly Random _random = new Random();

        // Генерация случайной строки длиной 10 из ASCII символов (например, от 'a' до 'z', 'A' до 'Z', '0' до '9')
        public static string GenerateRandomString(int length = 10)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            char[] stringChars = new char[length];

            for (int i = 0; i < length; i++)
            {
                stringChars[i] = chars[_random.Next(chars.Length)];
            }

            return new string(stringChars);
        }
    }
    public static class Model
    {
        private static readonly Stopwatch stopwatch = new Stopwatch();
        public static long[] StartGetTest(ITestable<int, int> dataStruct, int minTenPower, int maxTenPower)
        {
            Random random = new Random();
            const int RAND_START = 1;
            int RAND_END = 1000;
            const uint operations = 50;
            long[] testResults = new long[maxTenPower-minTenPower+1];
            
            for (int j = 0; j < (int)Math.Pow(10, minTenPower-1); j++)
            {
                RAND_END = (int)Math.Pow(10, j);
                dataStruct.Put(random.Next(RAND_START, RAND_END), random.Next(RAND_START, RAND_END));
            }
            
            for (int i = minTenPower; i <= maxTenPower; i++)
            {
                /*
                for (int j = 0; j < (int)Math.Pow(10, i); j++)
                {
                    _arrayDeque.Add(random.Next(RAND_START, RAND_END));
                    _linkedList.Add(random.Next(RAND_START, RAND_END));
                }
                */
                for (int j = (int)Math.Pow(10, i-1); j < (int)Math.Pow(10, i); j++)
                {
                    RAND_END = (int)Math.Pow(10, j);
                    dataStruct.Put(random.Next(RAND_START, RAND_END), random.Next(RAND_START, RAND_END));
                }

                stopwatch.Restart();
                for (int j = 0; j <= operations; j++)
                {
                    RAND_END = (int)Math.Pow(10, j);
                    dataStruct.Get( random.Next(RAND_START, RAND_END));
                }
                stopwatch.Stop();
                testResults[i - minTenPower] = stopwatch.ElapsedMilliseconds;
            }
            return testResults;
        }
        
        public static long[] StartPutTest(ITestable<int, int> dataStruct, int minTenPower, int maxTenPower)
        {
            Random random = new Random();
            const int RAND_START = 1;
            int RAND_END = 1000;
            long[] testResults = new long[maxTenPower-minTenPower+1];
            
            for (int i = minTenPower; i <= maxTenPower; i++)
            {
                stopwatch.Restart();
                for (int j = 0; j < (int)Math.Pow(10,i); j++)
                {
                    RAND_END = (int)Math.Pow(10, j);
                    dataStruct.Put(random.Next(RAND_START, RAND_END), random.Next(RAND_START, RAND_END));
                }
                stopwatch.Stop();
                //testResults[i - minTenPower] = stopwatch.ElapsedTicks;
                testResults[i - minTenPower] = stopwatch.ElapsedMilliseconds;
            }
            return testResults;
        }

        public static long[] StartRemoveTest(ITestable<int, int> dataStruct, int minTenPower, int maxTenPower)
        {
            Random random = new Random();
            const int RAND_START = 1;
            int RAND_END = 1000;
            const uint operations = 20;
            long[] testResults = new long[maxTenPower-minTenPower+1];
            
            for (int j = 0; j < (int)Math.Pow(10, minTenPower-1); j++)
            {
                RAND_END = (int)Math.Pow(10, j);
                dataStruct.Put(random.Next(RAND_START, RAND_END), random.Next(RAND_START, RAND_END));
            }
            
            for (int i = minTenPower; i <= maxTenPower; i++)
            {
                for (int j = (int)Math.Pow(10, i-1); j < (int)Math.Pow(10, i); j++)
                {
                    RAND_END = (int)Math.Pow(10, j);
                    dataStruct.Put(random.Next(RAND_START, RAND_END), random.Next(RAND_START, RAND_END));
                }
                
                stopwatch.Restart();
                for (int j = 0; j <= operations; j++)
                {
                    RAND_END = (int)Math.Pow(10, j);
                    dataStruct.Remove(random.Next(RAND_START, RAND_END));
                }
                stopwatch.Stop();
                testResults[i - minTenPower] = stopwatch.ElapsedMilliseconds;
            }
            return testResults;
        }
    }
}