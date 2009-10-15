using System;
using System.IO;

namespace Metaheuristics
{
    class Program
    {
        static void Main(string[] args)
        {
            var reader = new InstanceReader();
            try
            {
                reader.ReadInstance("res/chr18a.dat");
                reader.PrintInstances();
            }
            catch(FileNotFoundException fnfe)
            {
                Console.WriteLine(fnfe.Message);
            }
            Console.ReadLine();
        }
    }
}
