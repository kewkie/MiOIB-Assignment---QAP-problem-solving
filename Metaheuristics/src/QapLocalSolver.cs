using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Metaheuristics
{
    delegate int[] GenerateInitialSolution();
    delegate int[] GenerateNeighbour(int number);

    class QapLocalSolver
    {
        QapInstance _instance;
        int _seed;

        GenerateInitialSolution _initialSolutionGenerator;
        GenerateNeighbour _neighbourGenerator;


        public QapLocalSolver(QapInstance instance, int seed,
            InitialSolution initialSolution, NeighbourhoodType neighbourhoodType)
        {
            _instance = instance;
            switch (initialSolution)
            {
                case InitialSolution.Random: _initialSolutionGenerator = GenerateRandomSolution; break;
                default: _initialSolutionGenerator = GenerateRandomSolution; break;
            }
            switch (neighbourhoodType)
            {
                case NeighbourhoodType.Two_Swap: break;
                case NeighbourhoodType.Three_Swap: break;
                default: break;
            }
            _seed = seed;
        }

        public int[] Solve()
        {
            int[] initialSolution = _initialSolutionGenerator();
            var perm = new Neighbourhood(initialSolution, NeighbourhoodType.Two_Swap);
            foreach (var neighborhood in perm)
            {
                foreach (var i in neighborhood)
                {
                    Console.Write(i + " ");
                }
                Console.WriteLine();
            }

            return initialSolution;
        } 



        public int Evaluate(int[] solution)
        {
            int totalCost = 0;
            for (int i = 0; i < solution.Length; i++)
            {
                for (int j = 0; j < solution.Length; j++)
                {
                    totalCost += _instance.DistanceMatrix[i*solution.Length + j] * _instance.CostMatrix[solution[i]*solution.Length + j];    
                }                   
            }
            Console.WriteLine("Evaluation = {0}", totalCost);
            return totalCost;
        }

        public int[] GenerateRandomSolution()
        {
            int[] solution = new int[_instance.Size];

            for (int i = 0; i < _instance.Size ; i++)
			    solution[i] = i;
            
            Random r = new Random(_seed);
            
            for (int i = _instance.Size; i >= 2; i--)
            {
                int temp = solution[i - 1];
                int spot = r.Next() % i;
                solution[i - 1] = solution[spot];
                solution[spot] = temp;
            }

            for (int i = 0; i < solution.Length; i++)
                Console.WriteLine("{0}: {1}", i, solution[i]);

            return solution;
        }

        public int[] GenerateGreedyHeuristicSolution()
        {
            int size = _instance.Size;
            int sizexsize = _instance.DistanceMatrix.Length;
            int[] distanceClone = new int[sizexsize];
            int[] costClone = new int[sizexsize];
            for (int i = 0; i < sizexsize; i++)
            {
                distanceClone[i] = _instance.DistanceMatrix[i];
                costClone[i] = _instance.CostMatrix[i];
            }

            int[] solution = new int[size];

            for (int i = 0; i < size; i++)
            {
                int maxPosition = 0;
                for (int j = 0; j < sizexsize; j++)
                    if (distanceClone[size * i + j] > distanceClone[maxPosition])
                        maxPosition = size * i + j;
                solution[maxPosition/size] = maxPosition % size;
                for (int j = 0; j < sizexsize; j++)
                    ;

            }
             

            return solution;
        }

    }
}
