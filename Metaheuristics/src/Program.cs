using System;
using System.IO;

namespace Metaheuristics
{
    class Program
    {
        static void Main(string[] args)
        {
            QapInstance instance;
            try
            {
                instance = InstanceIO.ReadInstance("res/chr18a.dat");
                InstanceIO.PrintInstance(instance);
            }
            catch(FileNotFoundException fnfe)
            {
                Console.WriteLine(fnfe.Message);
            }
            Console.ReadLine();
        }
    }
}
