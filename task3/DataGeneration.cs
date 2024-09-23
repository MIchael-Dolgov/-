using System;

namespace DataGeneration
{
    public static class Generators
    {
        public static int[] RandomByModulo(int len)
        {
            int[] array = new int[len];
            Random random = new Random();
            for (int i = 0; i < len; i++)
            {
                array[i] = random.Next(0, 1000);
            }
            return array;
        }
        public static double[] RandomDouble(int len)
        {
            double[] array = new double[len];
            Random random = new Random();
            for (int i = 0; i < len; i++)
            {
                array[i] = (double)random.Next(1, 100) / 100;
            }
            return array;
        }

        public static int[] RandomWithSubArrs(int len)
        {
            Random random = new Random();
            int modulo = random.Next(0, len);
            int newLen = random.Next(2, len) % modulo;
            if (newLen < 2) newLen = 2;
            int[] array = new int[len];
            int countOfArray = 0;

            int i = 0;
            while (i < len)
            {
                int exp = random.Next(0, 1000);
                int elementBase = 0;
                countOfArray++;

                while (i < len && i < newLen * countOfArray)
                {
                    elementBase++;
                    array[i] = elementBase * exp;
                    i++;
                }
            }

            return array;
        }

        public static int[] SortedWithPermutations(int len)
        {
            int[] array = new int[len];
            for (int i = 0; i < len; i++) array[i] = i;

            Random random = new Random();
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

        public static int[] RandomizeSortedWithPermutations(int len)
        {
            int[] array = SortedWithPermutations(len);
            Random random = new Random();
            int indexOfRepeat = random.Next(0, len - 1);
            int countOfRepeat = random.Next(0, len / 3);

            while (countOfRepeat > 0)
            {
                int randomIndex = random.Next(0, array.Length - 1);
                if (array[randomIndex] != array[indexOfRepeat])
                {
                    array[randomIndex] = array[indexOfRepeat];
                    countOfRepeat--;
                }

            }
            return array;
        }
    }
}