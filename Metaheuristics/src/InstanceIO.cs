using System;
using System.IO;
using System.Linq;

namespace Metaheuristics
{
    static class InstanceIO
    {
        public static QapInstance ReadInstance(string fileName)
        {
            TextReader instanceData;
            ReaderMode readerMode = ReaderMode.Count;
            QapInstance instance = null;
            var dataFileName = fileName + ".dat";
            if (File.Exists(dataFileName))
            {
                instanceData = new StreamReader(dataFileName);
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
                            instance.Name = fileName;
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
                throw new FileNotFoundException("File " + fileName + " not found, fool");
            }
            if(File.Exists(fileName+".sln"))
            {
                var solutionData = new StreamReader(fileName+".sln");
                var scoreLine = solutionData.ReadLine();
                var soluionLine = solutionData.ReadLine();

                var score = scoreLine
                    .Split(' ')
                    .Where(x => !string.IsNullOrEmpty(x))
                    .ToArray()[1];

                var solution = soluionLine
                    .Split(' ')
                    .Where(x => !string.IsNullOrEmpty(x))
                    .Select(x => Int32.Parse(x))
                    .ToArray();

                if (instance != null)
                {
                    instance.OptimalScore = Int32.Parse(score);
                    instance.OptimalSolution = solution;
                }
                solutionData.Close();
            }
            else
            {
                if (instance != null) instance.OptimalScore = -1;
            }
            instanceData.Close();
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

        public static void PrintInstance(this QapInstance instance, PrintInstanceMode mode)
        {
            if (mode == PrintInstanceMode.Verbose)
            {
                PrintSquareArray(instance.DistanceMatrix, instance.Size);
                PrintSquareArray(instance.CostMatrix, instance.Size);
                
            }
            else
            {
                Console.WriteLine("Instance name: {0}", instance.Name);
                Console.WriteLine("Instance size: {0}", instance.Size);
            }
            Console.WriteLine("Optimal score: {0}", instance.OptimalScore);
        }
    }
}
