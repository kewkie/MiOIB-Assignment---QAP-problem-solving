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
                var qls = new QapLocalSolver(instance)
                              {
                                  InitialSolution = InitialSolution.Random,
                                  NeighbourhoodType = NeighbourhoodType.TwoSwap,
                                  SearchType = LocalSearchType.Greedy,
                                  Seed = 0
                              };
                Console.WriteLine("Final evaluation: {0}",qls.Evaluate(qls.Solve()));
            }
            catch(FileNotFoundException fnfe)
            {
                Console.WriteLine(fnfe.Message);
            }
            Console.ReadLine();
        }
    }
}
