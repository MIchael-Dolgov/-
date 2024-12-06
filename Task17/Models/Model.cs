using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Task17.Models
{
    public static class Model
    {
        private static readonly Stopwatch stopwatch = new Stopwatch();
        public static long[] StartGetTest(ITestable<int> dataStruct, int minTenPower, int maxTenPower)
        {
            Random random = new Random();
            const int RAND_START = 1;
            const int RAND_END = 1000;
            const uint operations = 20;
            long[] testResults = new long[maxTenPower-minTenPower+1];
            
            for (int j = 0; j < (int)Math.Pow(10, minTenPower-1); j++)
            {
                dataStruct.Add(random.Next(RAND_START, RAND_END));
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
                    dataStruct.Add(random.Next(RAND_START, RAND_END));
                }
                
                stopwatch.Restart();
                for (int j = 0; j <= operations; j++)
                {
                    dataStruct.Get(random.Next(RAND_START, RAND_END));
                }
                stopwatch.Stop();
                testResults[i - minTenPower] = stopwatch.ElapsedMilliseconds;
            }
            return testResults;
        }

        public static long[] StartSetTest(ITestable<int> dataStruct, int minTenPower, int maxTenPower)
        {
            Random random = new Random();
            const int RAND_START = 1;
            const int RAND_END = 1000;
            const uint operations = 20;
            long[] testResults = new long[maxTenPower-minTenPower+1];
            
            for (int j = 0; j < (int)Math.Pow(10, minTenPower-1); j++)
            {
                dataStruct.Add(random.Next(RAND_START, RAND_END));
            }
            
            for (int i = minTenPower; i <= maxTenPower; i++)
            {
                for (int j = (int)Math.Pow(10, i-1); j < (int)Math.Pow(10, i); j++)
                {
                    dataStruct.Add(random.Next(RAND_START, RAND_END));
                }
                
                stopwatch.Restart();
                for (int j = 0; j <= operations; j++)
                {
                    dataStruct.Set(random.Next(0, (int)Math.Pow(10, i)-1),
                        random.Next(RAND_START, RAND_END));
                }
                stopwatch.Stop();
                testResults[i - minTenPower] = stopwatch.ElapsedMilliseconds;
            }
            return testResults;
        }

        public static long[] StartAddTest(ITestable<int> dataStruct, int minTenPower, int maxTenPower)
        {
            Random random = new Random();
            const int RAND_START = 1;
            const int RAND_END = 1000;
            long[] testResults = new long[maxTenPower-minTenPower+1];
            
            for (int i = minTenPower; i <= maxTenPower; i++)
            {
                stopwatch.Restart();
                for (int j = 0; j < (int)Math.Pow(10,i); j++)
                {
                    dataStruct.Add(random.Next(RAND_START, RAND_END));
                }
                stopwatch.Stop();
                testResults[i - minTenPower] = stopwatch.ElapsedMilliseconds;
            }
            return testResults;
        }

        public static long[] StartInsertionTest(ITestable<int> dataStruct, int minTenPower, int maxTenPower)
        {
            Random random = new Random();
            const int RAND_START = 1;
            const int RAND_END = 1000;
            const uint operations = 20;
            long[] testResults = new long[maxTenPower-minTenPower+1];
            
            for (int j = 0; j < (int)Math.Pow(10, minTenPower-1); j++)
            {
                dataStruct.Add(random.Next(RAND_START, RAND_END));
            }
            
            for (int i = minTenPower; i <= maxTenPower; i++)
            {
                for (int j = (int)Math.Pow(10, i-1); j < (int)Math.Pow(10, i); j++)
                {
                    dataStruct.Add(random.Next(RAND_START, RAND_END));
                }
                
                stopwatch.Restart();
                for (int j = 0; j <= operations; j++)
                {
                    dataStruct.Add(random.Next(0, (int)Math.Pow(10, i)-1),
                        random.Next(RAND_START, RAND_END));
                }
                stopwatch.Stop();
                testResults[i - minTenPower] = stopwatch.ElapsedMilliseconds;
            }
            return testResults; 
        }

        public static long[] StartRemoveTest(ITestable<int> dataStruct, int minTenPower, int maxTenPower)
        {
            Random random = new Random();
            const int RAND_START = 1;
            const int RAND_END = 1000;
            const uint operations = 20;
            long[] testResults = new long[maxTenPower-minTenPower+1];
            
            for (int j = 0; j < (int)Math.Pow(10, minTenPower-1); j++)
            {
                dataStruct.Add(random.Next(RAND_START, RAND_END));
            }
            
            for (int i = minTenPower; i <= maxTenPower; i++)
            {
                for (int j = (int)Math.Pow(10, i-1); j < (int)Math.Pow(10, i); j++)
                {
                    dataStruct.Add(random.Next(RAND_START, RAND_END));
                }
                
                stopwatch.Restart();
                for (int j = 0; j <= operations; j++)
                {
                    dataStruct.Remove(random.Next(RAND_START, RAND_END));
                }
                stopwatch.Stop();
                testResults[i - minTenPower] = stopwatch.ElapsedMilliseconds;
            }
            return testResults;
        }
    }
}