using System;
using System.IO;

namespace Metaheuristics
{
    class Program
    {
        static void Main()
        {
            try
            {
                //instance = InstanceIO.ReadInstance("res/chr18a.dat");
                //instance = InstanceIO.ReadInstance("res/els19.dat");
                QapInstance instance = InstanceIO.ReadInstance("res/esc16a.dat");
                InstanceIO.PrintInstance(instance);
                var qls = new QapLocalSolver(instance, 0, InitialSolution.Random, NeighbourhoodType.TwoSwap);
                qls.Evaluate(qls.Solve());
            }
            catch(FileNotFoundException fnfe)
            {
                Console.WriteLine(fnfe.Message);
            }
            Console.ReadLine();
        }
    }
}
