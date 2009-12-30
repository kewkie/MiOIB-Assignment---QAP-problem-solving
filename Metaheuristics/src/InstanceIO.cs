﻿using System;
using System.IO;

namespace Metaheuristics
{
    static class InstanceIO
    {
        public static QapInstance ReadInstance(string fileName)
        {
            TextReader instanceData;
            ReaderMode readerMode = ReaderMode.Count;
            QapInstance instance = null;
            if (File.Exists(fileName))
            {
                instanceData = new StreamReader(fileName);
                var strLine = String.Empty;
                int instanceSize = 0;
                int numbersRead = 0;
                var separators = new[] { ' ' };
                
                while (strLine != null)
                {
                    strLine = instanceData.ReadLine();
                    strLine = strLine.Trim();
                    if (strLine == String.Empty)
                    {
                        continue;
                    }
                    switch (readerMode)
                    {
                        case ReaderMode.Count:
                            instanceSize = Int32.Parse(strLine);
                            instance = new QapInstance(instanceSize);
                            readerMode = ReaderMode.Distances;

                            break;
                        case ReaderMode.Distances:
                            if (instance != null)
                            {
                                var splittedNumbers = strLine.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                                for(int i = 0; i < splittedNumbers.Length; i++)
                                {
                                    instance.DistanceMatrix[numbersRead++] = Int32.Parse(splittedNumbers[i]);
                                }
                            }
                                
                            else
                                throw new InvalidOperationException("Instance should not be null");
                            if (numbersRead == instanceSize*instanceSize)
                            {
                                readerMode = ReaderMode.Costs;
                                numbersRead = 0;
                            }
                            break;
                        case ReaderMode.Costs:
                            if (instance != null)
                            {
                                var splittedNumbers = strLine.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                                for (int i = 0; i < splittedNumbers.Length; i++)
                                {
                                    instance.CostMatrix[numbersRead++] = Int32.Parse(splittedNumbers[i]);
                                }
                            }
                            else
                                throw new InvalidOperationException("Instance should not be null");
                            if (numbersRead == instanceSize*instanceSize)
                            {
                                strLine = null;
                            }
                            break;
                    }
                }
            }
            else
            {
                throw new FileNotFoundException("File not found, fool");
            }
            return instance;
        }

        public static void PrintSquareArray(int[] array, int size)
        {
            int max = 0;
            foreach (int i in array)
                if (i > max)
                    max = i;
            int elementRank = 0;
            for (int i = 0; i < 10; i++)
            {
                if (max > Math.Pow(10, i))
                    elementRank++;
                else
                    break;
            }
            int sizeRank = 0;
            for (int i = 0; i < 10; i++)
            {
                if (size > Math.Pow(10, i))
                    sizeRank++;
                else
                    break;
            }
            int padding = Math.Max(elementRank + 1, sizeRank + 1);
            Console.Write("|".PadLeft(sizeRank+1));
            for (int i = 0; i < size; i++)
                Console.Write(i.ToString().PadLeft(padding));
            Console.WriteLine();
            Console.Write("+".PadLeft(sizeRank+1,'-') + "".PadRight(size*padding,'-'));
            Console.WriteLine();
            for (int i = 0; i < size; i++)
            {
                Console.Write(i.ToString().PadLeft(sizeRank) + "|");
                for (int j = 0; j < size; j++)
                {
                    Console.Write(array[size*i+j].ToString().PadLeft(padding));
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public static void PrintInstance(QapInstance instance)
        {
            PrintSquareArray(instance.DistanceMatrix, instance.Size);
            PrintSquareArray(instance.CostMatrix, instance.Size);
        }
    }
}
