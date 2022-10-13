using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Sockets;

namespace DirectMerge
{
    public static class DirectMerge
    {
        private static string PathA = "C:\\Users\\R2D2\\source\\repos\\DirectMerge\\A.txt";
        private static string PathB = "C:\\Users\\R2D2\\source\\repos\\DirectMerge\\B.txt";
        private static string PathC = "C:\\Users\\R2D2\\source\\repos\\DirectMerge\\C.txt";

        private static long NumbersAmount = 0;

        public static void Merge()
        {
            GetAmountOfNumbers();
            Console.WriteLine(NumbersAmount);
            long iteration = 1;
            while (iteration < NumbersAmount)
            {
                SplitInput(iteration);
                MergeSubFiles(iteration);
                File.WriteAllText(PathB, "");
                File.WriteAllText(PathC, "");
                iteration *= 2;
            }
        }
        static void GetAmountOfNumbers()
        {
            StreamReader srA = new StreamReader(PathA);
            while (!srA.EndOfStream)
            {
                try
                {
                    if (srA.Peek() == ' ')
                    {
                        NumbersAmount++;
                    }
                }
                catch
                {
                    NumbersAmount++;
                }

                char a = (char)srA.Read();
            }
            srA.Close();
        }

        static void SplitInput(long iteration)
        {
            StreamReader srA = new StreamReader(PathA);
            StreamWriter swB = new StreamWriter(PathB);
            StreamWriter swC = new StreamWriter(PathC);
            bool writeInB = true;
            long count = 0;
            string number = "";
            while (!srA.EndOfStream)
            {
                if (srA.Peek() == ' ')
                {
                    count++;
                    number += (char)srA.Read();
                    if (writeInB)
                    {
                        swB.Write(number);
                    }
                    else
                    {
                        swC.Write(number);
                    }
                    number = "";

                    if (count >= iteration)
                    {
                        if (writeInB)
                            writeInB = false;
                        else
                            writeInB = true;
                        count = 0;
                    }
                }
                else
                {
                    number += (char)srA.Read();
                }
            }
            srA.Close();
            swB.Close();
            swC.Close();
        }

        static void MergeSubFiles(long iteration)
        {
            StreamWriter swA = new StreamWriter(PathA);
            StreamReader srB = new StreamReader(PathB);
            StreamReader srC = new StreamReader(PathC);
            int b = -1;
            int c = -1;
            int counterB = 0;
            int counterC = 0;

            while (!srB.EndOfStream && !srC.EndOfStream)
            {
                if (b == -1)
                {
                    string nnumber = ReadNumbers(srB);
                    b = Convert.ToInt32(nnumber);
                    counterB++;
                }
                if (c == -1)
                {
                    string nnumber = ReadNumbers(srC);
                    c = Convert.ToInt32(nnumber);
                    counterC++;
                }

                if (c < b)
                {
                    swA.Write(c.ToString() + " ");
                    if (counterC == iteration)
                    {
                        if (counterB <= iteration)
                        {
                            swA.Write(b.ToString() + " ");
                            b = -1;
                            counterB++;
                        }

                        for (; counterB <= iteration; counterB++)
                        {
                            swA.Write(ReadNumbers(srB) + "");
                        }

                        counterB = 0;
                        counterC = 0;
                    }

                    c = -1;
                }

                else
                {
                    swA.Write(b.ToString() + " ");
                    if (counterB == iteration)
                    {
                        if (counterC <= iteration)
                        {
                            swA.Write(c.ToString() + " ");
                            c = -1;
                            counterC++;
                        }

                        for (; counterC <= iteration; counterC++)
                        {
                            swA.Write(ReadNumbers(srC) + " ");
                        }

                        counterB = 0;
                        counterC = 0;
                    }

                    b = -1;
                }

            }

            if (b != -1)
            {
                swA.Write(b.ToString() + " ");
            }
            if (c != -1)
            {
                swA.Write(c.ToString() + " ");
            }

            while (!srB.EndOfStream)
            {
                swA.Write(ReadNumbers(srB) + " ");
            }
            while (!srC.EndOfStream)
            {
                swA.Write(ReadNumbers(srC) + " ");
            }
            swA.Close();
            srB.Close();
            srC.Close();
        }

        static string ReadNumbers(StreamReader sr)
        {
            string number = "";
            if(!sr.EndOfStream)
            {
                char element = (char)sr.Read();
                while (element != ' ')
                {
                    number += element;
                    element = (char)sr.Read();
                }
            }
            return number;
        }
    }
}
