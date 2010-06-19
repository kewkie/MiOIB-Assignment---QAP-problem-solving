using System;
using System.Linq;

namespace Metaheuristics
{
    delegate int[] GenerateInitialSolution(QapInstance instance);
    delegate int[] GenerateNeighbour(int number);
    
    internal abstract class QapSolver
    {
        //private readonly QapInstance _instance;
        protected InitialSolution _initialSolution;
        protected GenerateInitialSolution _initialSolutionGenerator;
        private static readonly Random Seed = new Random((int)DateTime.Now.Ticks);

        public InitialSolution InitialSolution
        {
            get
            {
                return _initialSolution;
            }
            set
            {
                _initialSolution = value;
                switch (value)
                {
                    case InitialSolution.Random:
                        _initialSolutionGenerator = GenerateRandomSolution;
                        break;
                    case InitialSolution.Heuristic:
                        _initialSolutionGenerator = GenerateGreedyHeuristicSolution;
                        break;
                    case InitialSolution.AntiHeuristic:
                        _initialSolutionGenerator = GenerateGreedyAntiHeuristicSolution;
                        break;
                    default:
                        _initialSolutionGenerator = GenerateRandomSolution;
                        break;
                }

            }
        }
        
        public abstract int[] Solve(QapInstance instance);

        public abstract void Reset();
        
        public static int Evaluate(int[] solution, QapInstance instance)
        {
            int totalCost = 0;
            for (int i = 0; i < solution.Length; i++)
            {
                for (int j = 0; j < solution.Length; j++)
                {
                    totalCost += instance.DistanceMatrix[i*solution.Length + j] * instance.CostMatrix[solution[i]*solution.Length + solution[j]];    
                }                   
            }
            return totalCost;
        }

        private static int[] GenerateRandomSolution(QapInstance instance)
        {
            int[] solution = new int[instance.Size];

            for (int i = 0; i < instance.Size; i++)
                solution[i] = i;

            //Random r = new Random(Seed.Next());

            for (int i = instance.Size; i >= 2; i--)
            {
                int temp = solution[i - 1];
                int spot = Seed.Next() % i;
                solution[i - 1] = solution[spot];
                solution[spot] = temp;
            }

            //for (int i = 0; i < solution.Length; i++)
            //   Console.Write("{0} ", solution[i]);
            //Console.WriteLine();

            return solution;
        }

        private static int[] GenerateGreedyHeuristicSolution(QapInstance instance)
        {
            var solution = new int[instance.Size];
            for (int i = 0; i < solution.Length; i++)
            {
                solution[i] = -1;
            }


            while (true)
            {
                var highest = Int32.MinValue;
                var highestPos = -1;
                var lowest = Int32.MaxValue;
                var lowestPos = -1;

                for (int i = 0; i < instance.Size * instance.Size; i++)
                {
                    if ((i % instance.Size != i / instance.Size) && instance.DistanceMatrix[i] > highest && (solution[i % instance.Size] == -1) &&
                        (solution[i / instance.Size] == -1))
                    {
                        highest = instance.DistanceMatrix[i];
                        highestPos = i;
                    }
                    if ((i % instance.Size != i / instance.Size) && (instance.CostMatrix[i]) < lowest && (!solution.Contains(i % instance.Size) && (!solution.Contains(i / instance.Size))))
                    {
                        lowest = instance.CostMatrix[i];
                        lowestPos = i;
                    }

                }

                if (highestPos == -1)
                    break;

                solution[highestPos / instance.Size] = lowestPos / instance.Size;
                solution[highestPos % instance.Size] = lowestPos % instance.Size;
            }

            if (instance.Size % 2 == 1)
            {
                int index = Array.FindIndex(solution, 0, x => x == -1);
                solution[index] = (instance.Size) * ((instance.Size - 1) / 2) - solution.Sum() - 1;
            }

            return solution;
        }

        private static int[] GenerateGreedyAntiHeuristicSolution(QapInstance instance)
        {
            var solution = new int[instance.Size];
            for (int i = 0; i < solution.Length; i++)
            {
                solution[i] = -1;
            }


            while (true)
            {
                var highestDistance = Int32.MinValue;
                var highestDistancePos = -1;
                var highestCost = Int32.MinValue;
                var highestCostPos = -1;

                for (int i = 0; i < instance.Size * instance.Size; i++)
                {
                    if ((i % instance.Size != i / instance.Size) && instance.DistanceMatrix[i] > highestDistance && (solution[i % instance.Size] == -1) &&
                        (solution[i / instance.Size] == -1))
                    {
                        highestDistance = instance.DistanceMatrix[i];
                        highestDistancePos = i;
                    }
                    if ((i % instance.Size != i / instance.Size) && (instance.CostMatrix[i]) > highestCost && (!solution.Contains(i % instance.Size) && (!solution.Contains(i / instance.Size))))
                    {
                        highestCost = instance.CostMatrix[i];
                        highestCostPos = i;
                    }

                }

                if (highestDistancePos == -1)
                    break;

                solution[highestDistancePos / instance.Size] = highestCostPos / instance.Size;
                solution[highestDistancePos % instance.Size] = highestCostPos % instance.Size;
            }

            if (instance.Size % 2 == 1)
            {
                int index = Array.FindIndex(solution, 0, x => x == -1);
                solution[index] = (instance.Size) * ((instance.Size - 1) / 2) - solution.Sum() - 1;
            }

            return solution;
        }
    }
}