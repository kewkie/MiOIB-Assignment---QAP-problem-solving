using System;
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
                int rowsRead = 0;
                
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
                            instance.DistanceMatrix[rowsRead++] = FillArrayRow(strLine);
                            if (rowsRead == instanceSize)
                            {
                                readerMode = ReaderMode.Costs;
                                rowsRead = 0;
                            }
                            break;
                        case ReaderMode.Costs:
                            instance.CostMatrix[rowsRead++] = FillArrayRow(strLine);
                            if (rowsRead == instanceSize)
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

        private static int[] FillArrayRow(string row)
        {
            var separators = new[]{' '};
            var splittedNumbers = row.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            var parsedNumbers = new int[splittedNumbers.Length];
            for (int i = 0; i < splittedNumbers.Length; i++)
            {
                parsedNumbers[i] = Int32.Parse(splittedNumbers[i]);
            }
            return parsedNumbers;
        }

        public static void PrintArray(int[][] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                for (int j = 0; j < array[i].Length; j++)
                {
                    Console.Write("{0} ", array[i][j]);
                }
                Console.WriteLine();
            }
        }

        public static void PrintInstance(QapInstance instance)
        {
            PrintArray(instance.DistanceMatrix);
            PrintArray(instance.CostMatrix);
        }
    }
}
