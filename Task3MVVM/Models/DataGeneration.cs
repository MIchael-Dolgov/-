using System;

namespace Task3MVVM.Models;

    public static class Generators
    {
        public static int[] RandomByModulo(int len, int mod = 10)
        {
            int[] array = new int[len];
            Random random = new Random();
            for (int i = 0; i < len; i++)
            {
                array[i] = random.Next(0, mod);
            }
            return array;
        }
        public static int[] RandomWithSubArrs(int len, int modConst = 10)
        {
            int[] array = new int[len];
            int freeLenght = array.Length;
            Random random = new Random();
            while (freeLenght != 0)
            {
                int NextRandLowerNum = 0;
                int SubArrLen = random.Next(1, freeLenght);
                for (int i = 0; i < SubArrLen; i++)
                {
                    NextRandLowerNum= random.Next(NextRandLowerNum, 100 + NextRandLowerNum);
                    array[array.Length - freeLenght + i] = NextRandLowerNum;
                }
                freeLenght -= SubArrLen;
            }
            return array;
        }

        public static int[] SortedWithPermutations(int len)
        {
            int[] array = new int[len];
            int NextRandLowerNum = 0;
            Random random = new Random();
            
            for (int i = 0; i < len; i++)
            {
                NextRandLowerNum = random.Next(NextRandLowerNum, 10 + NextRandLowerNum);
                array[i] = NextRandLowerNum;
            }

            // Количество перестановок на массив
            int countOfSwap = random.Next(0, len/3);
            
            for (int i = 0; i < countOfSwap; i++)
            {
                int firstIndex = random.Next(0, array.Length - 1);
                int secondIndex = random.Next(0, array.Length - 1);
                int temp = array[firstIndex];
                array[firstIndex] = array[secondIndex];
                array[secondIndex] = temp;
            }
            return array;
        }
        
        public static int[] ArrayWithRepeats(int len, int precent = 10)
        {
            int[] array = new int[len];
            int repeats = (int)(array.Length / precent);
            Random random = new Random();
            int repNum = random.Next(200, 1000);
            for (int i = 0; i <= repeats; i++)
            {
                array[i] = repNum;
            }
            for (int i = repeats + 1; i < array.Length; i++)
            {
                array[i] = random.Next(0, 199);
            }
            return array;
        }
    }