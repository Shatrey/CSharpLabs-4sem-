using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1Sharp
{
    static class Web
    {
        private static int size ;

        private static double[,] disMatrix; 

        public static int Size { get => size; set => size = value; }

        public static void Start()
        {
            Console.Write("Type 0 to start with standart graph, 1 - to create new : ");
            int choice = Int32.Parse(Console.ReadLine());
           

            switch (choice)
            {
                case 1:
                    UserMatrixInput();
                    break;
                default:
                    StandartMatrixInput();
                    break;
            }
                
        }

        private static void StandartMatrixInput()
        {
            Size = 12;
            disMatrix = new double[Size, Size];
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    disMatrix[i, j] = 0;
                    if (i == j) disMatrix[i, j] = 1;
                }
            }
            disMatrix[0, 1] = 0.07;     disMatrix[0, 6] = 0.08;     disMatrix[1, 8] = 0.07;     disMatrix[2, 7] = 0.06;
            disMatrix[2, 10] = 0.04;    disMatrix[3, 6] = 0.07;     disMatrix[3, 7] = 0.09;     disMatrix[4, 5] = 0.09;
            disMatrix[4, 6] = 0.06;     disMatrix[5, 8] = 0.03;     disMatrix[6, 9] = 0.02;     disMatrix[7, 9] = 0.04;
            disMatrix[8, 9] = 0.09;     disMatrix[8, 10] = 0.02;    disMatrix[9, 11] = 0.05;    disMatrix[10, 11] = 0.06;
            for (int i = 0; i < Size - 1; i++)
            {
                for (int j = i + 1; j < Size; j++)
                {
                    disMatrix[j, i] = disMatrix[i, j];
                }
            }
        }

        private static void UserMatrixInput()
        {
            Console.Write("Input number of vertices : ");
            Size = Int32.Parse(Console.ReadLine());

            bool[] vertexConnected = new bool[size];
            disMatrix = new double[Size, Size];

            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    disMatrix[i, j] = 0;
                    if (i == j) disMatrix[i, j] = 1;
                }
            }

            Console.WriteLine("\nInput graph edges, S to stop");
            bool conditions = false;
            while (!conditions)
            {
                string str1, str2;
                int i, j;

                inputVertices(out str1, out str2);
                if (str1 == "S" || str2 == "S")
                {
                    Console.WriteLine("Ok, begin the calculations");
                    conditions = true;
                    for (int k = 0; k < Size; k++) 
                    {
                        if(vertexConnected[k]==false)
                        {
                            Console.WriteLine("Err! {0} vertex is not connected!!!", k);
                            conditions = false;
                        }
                    }
                    if (!conditions)
                        inputVertices(out str1, out str2);
                    else break;
                }
                i = Int32.Parse(str1);
                j = Int32.Parse(str2);
                vertexConnected[i] = true;
                vertexConnected[j] = true;

                double val;
                while (true)
                {
                    Console.Write("Chance to stole the message on this edge (>=0 and <1) : ");

                    val = Convert.ToDouble(Console.ReadLine(), new CultureInfo("eu-EU"));
                    val /= 10;
                    if ((val >= 0) && (val < 1))
                    {
                        break;
                    }
                    Console.WriteLine("Wrong input! Try again");
                }
                disMatrix[j, i] = disMatrix[i, j] = val;
            }
        }

        private static void inputVertices(out string str1,out string str2)
        {
            Console.Write("\nFirst vertex : ");
            str1 = Console.ReadLine();

            Console.Write("\nSecond vertex : ");
            str2 = Console.ReadLine();
        }

        private static int MinKey(double[] key, bool[] set)
        {
            double min = int.MaxValue;
            int minIndex = 0;

            for (int v = 0; v < Size; ++v)
            {
                if (set[v] == false && key[v] < min)
                {
                    min = key[v];
                    minIndex = v;
                }
            }

            return minIndex;
        }

        private static void Print(int[] parent)
        {
            Console.WriteLine("Edge      Weight");
            for (int i = 1; i < Size; ++i)
                Console.WriteLine("{0} - {1}    {2}", parent[i], i, disMatrix[i, parent[i]]);
        }

        public static void Prim()
        {
            int[] parent = new int[Size];
            double[] key = new double[Size];
            bool[] mstSet = new bool[Size];

            for (int i = 0; i < Size; ++i)
            {
                key[i] = int.MaxValue;
                mstSet[i] = false;
            }

            key[0] = 0;
            parent[0] = -1;

            for (int count = 0; count < Size - 1; ++count)
            {
                int u = MinKey(key, mstSet);
                mstSet[u] = true;

                for (int v = 0; v < Size; ++v)
                {
                    if (Convert.ToBoolean(disMatrix[u, v]) && mstSet[v] == false && disMatrix[u, v] < key[v])
                    {
                        parent[v] = u;
                        key[v] = disMatrix[u, v];
                    }
                }
            }

            Print(parent); // алгоритм Пріма побудував остове дерево, тому ми тепер можемо обчислити ймовірність HE перехвату повідомлення
            // Ймовірність HE перехвату буде рівна добутку всіх (1 - p[i,j]) - інформація з лекції де я знайшов своє завдання
            double result = 1;

            for(int i = 0; i<Size;i++)
            {
                if (key[i] == 0) continue;
                result *= (1 - key[i]);
            }
            result = 1 - result; // ймовірність ПЕРЕХВАТУ
            if(result <0 || result > 1)
            {
                Console.WriteLine("Inputed graph was wrong, not all verteces connected inside");
            }
            Console.WriteLine("Probability of message interception - {0}", result);
        }

    }
}
