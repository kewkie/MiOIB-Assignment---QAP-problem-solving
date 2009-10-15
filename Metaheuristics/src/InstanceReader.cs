using System;
using System.IO;

namespace Metaheuristics
{
    class InstanceReader
    {
        private TextReader _instance;
        private ReaderMode _readerMode = ReaderMode.Count;
        private int[][] _instanceDistances;
        private int[][] _instanceCosts;

        public void ReadInstance(string fileName)
        {
            if (File.Exists(fileName))
            {
                _instance = new StreamReader(fileName);
                var strLine = String.Empty;
                int instanceSize = 0;
                int rowsRead = 0;

                while (strLine != null)
                {
                    strLine = _instance.ReadLine();
                    strLine = strLine.Trim();
                    if (strLine == String.Empty)
                    {
                        continue;
                    }
                    switch (_readerMode)
                    {
                        case ReaderMode.Count:
                            instanceSize = Int32.Parse(strLine);
                            _instanceDistances = new int[instanceSize][];
                            _instanceCosts = new int[instanceSize][];
                            _readerMode = ReaderMode.Distances;

                            break;
                        case ReaderMode.Distances:
                            _instanceDistances[rowsRead] = FillArrayRow(strLine);
                            rowsRead++;
                            if (rowsRead == instanceSize)
                            {
                                _readerMode = ReaderMode.Costs;
                                rowsRead = 0;
                            }
                            break;
                        case ReaderMode.Costs:
                            _instanceCosts[rowsRead] = FillArrayRow(strLine);
                            rowsRead++;
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



        private static void PrintArray(int[][] array)
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

        public void PrintInstances()
        {
            PrintArray(_instanceDistances);
            PrintArray(_instanceCosts);
        }

    }
}
