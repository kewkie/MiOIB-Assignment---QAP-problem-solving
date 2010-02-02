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
                var instance = InstanceIO.ReadInstance("res/nug27.dat");
                InstanceIO.PrintInstance(instance);
                var qls = new QapLocalSolver(instance)
                              {
                                  InitialSolution = InitialSolution.AntiHeuristic,
                                  NeighbourhoodType = NeighbourhoodType.ThreeSwap,
                                  SearchType = LocalSearchType.Greedy,
                                  //Seed = DateTime.Now.Millisecond
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
