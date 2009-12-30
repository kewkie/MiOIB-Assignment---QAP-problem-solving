using System;

namespace Metaheuristics
{
    internal abstract class QapSolver
    {
        protected readonly QapInstance Instance;
        
        protected QapSolver(QapInstance instance)
        {
            Instance = instance;
        }

        public int InstanceSize
        {
            get
            {
                return Instance.Size;
            }
        }
        
        public abstract int[] Solve();
        
        public int Evaluate(int[] solution)
        {
            int totalCost = 0;
            for (int i = 0; i < solution.Length; i++)
            {
                for (int j = 0; j < solution.Length; j++)
                {
                    totalCost += Instance.DistanceMatrix[i*solution.Length + j] * Instance.CostMatrix[solution[i]*solution.Length + solution[j]];    
                }                   
            }
            //Console.WriteLine("Evaluation = {0}", totalCost);
            return totalCost;
        }
    }
}