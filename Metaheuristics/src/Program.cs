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
                //var instance = InstanceIO.ReadInstance("res/chr18a.dat");
                //var instance = InstanceIO.ReadInstance("res/els19.dat");
                var instance = InstanceIO.ReadInstance("res/nug18.dat");
                InstanceIO.PrintInstance(instance);
                var qls = new QapLocalSolver(instance)
                              {
                                  InitialSolution = InitialSolution.Random,
                                  NeighbourhoodType = NeighbourhoodType.TwoSwap,
                                  SearchType = LocalSearchType.Steepest,
                                  Seed = DateTime.Now.Millisecond
                              };
                var exp = new Experiment(qls);
                var results = exp.SolveWithTimeLimit(new TimeSpan(0, 1, 0), new TimeSpan(0,0,1));
            }
            catch(FileNotFoundException fnfe)
            {
                Console.WriteLine(fnfe.Message);
            }
            Console.ReadLine();
        }
    }
}
