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
                //instance = InstanceIO.ReadInstance("res/els19.dat");
                //instance = InstanceIO.ReadInstance("res/esc16a.dat");
                InstanceIO.PrintInstance(instance);
                QapLocalSolver qls = new QapLocalSolver(instance, 0, InitialSolution.Random, Neighbourhood.Two_Swap);
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
