using System;
using Task1.AlgStructures;

namespace Task1
{
    namespace AlgStructures
    {
        public class Matrix
        {
            public int[,] nums;
            public uint N;
            public uint M;

            public Matrix(uint x, uint y)
            {
                nums = new int[x, y];
                N = x;
                M = y;
            }

            public bool issymmetric()
            {
                //check for symmetry
                bool symmetric = true;
                for (uint i = 0; i < N && symmetric; i++)
                {
                    for (uint j = 0; j < M && symmetric; j++)
                    {
                        if (nums[i, j] != nums[j, i]) symmetric = false;
                    }
                }
                return symmetric;
            }

            public bool printmatr()
            {
                //check for symmetry
                bool symmetric = true;
                for (uint i = 0; i <= N && symmetric; i++)
                {
                    for (uint j = 0; j <= M && symmetric; j++)
                    {
                        Console.Write(nums[i, j] + " ");
                    }
                    Console.Write("\n");
                }
                return symmetric;
            }
        }

        public class Vector
        {
            public int[] nums;
            public uint lenght;

            public Vector(uint len)
            {
                nums = new int[len];
                lenght = len;
            }
        }
    }

    class Program
    {
        // Метод для преобразования строки в массив целых чисел
        static void Main()
        {
            uint line = 0;
            const string filePath = "C:\\Users\\michael\\source\\repos\\task1\\task1\\numbers.txt";

            string[] lines = File.ReadAllLines(filePath);
            uint N = uint.Parse(lines[line++].Trim().Split(" ")[0]);
            uint M = uint.Parse(lines[line++].Trim().Split(" ")[0]);

            string[] text;

            //Fill the matrix
            Matrix matr = new Matrix(N, M);

            for (uint i = 0; i < N; i++)
            {
                text = lines[line].Trim().Split(' ');
                if (text.Length!=M)
                {
                    Console.WriteLine("Matrix doesn't have same size dimension");
                    return;
                }
                for (uint j = 0; j<M; j++)
                {
                    matr.nums[i, j] = int.Parse(text[j]);
                }
                ++line;
            }

            //Fill the vector
            Vector vec = new Vector(M);

            text = lines[line].Trim().Split(" ");
            for (uint j = 0;  j < M; j++)
            {
                vec.nums[j] = int.Parse(text[j]);
            }

            //Next step after iniz
            Console.WriteLine("\t===Task1===\t");
            if (matr.issymmetric()!=true)
            {
                Console.WriteLine("Matrix is not symmetric");
                return;
            }

            //Calculate sqrt from formula: sqrt(vec*matr*vac^(T))

            Vector tempvec = new Vector(M);
            for (uint i = 0; i < N; i++) 
            {
                int sum = 0;
                for (uint j = 0; j < M; j++)
                {
                    sum += vec.nums[j] * matr.nums[i, j];
                }
                tempvec.nums[i] = sum;
            }
            {
                int sum = 0;
                for (uint i = 0; i < N; i++)
                {
                    sum += tempvec.nums[i] * vec.nums[i];
                }
                Console.WriteLine(sum);
                Console.WriteLine(Math.Sqrt(sum));
            }
        }
    }
}
