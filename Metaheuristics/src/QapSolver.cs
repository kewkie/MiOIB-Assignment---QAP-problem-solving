using System;
using System.Linq;

namespace Metaheuristics
{
    internal abstract class QapSolver
    {
        protected readonly QapInstance Instance;
        protected InitialSolution _initialSolution;
        protected GenerateInitialSolution _initialSolutionGenerator;
        
        protected QapSolver(QapInstance instance)
        {
            Instance = instance;
        }

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

        private int[] GenerateRandomSolution()
        {
            int[] solution = new int[Instance.Size];

            for (int i = 0; i < Instance.Size; i++)
                solution[i] = i;

            Random r = new Random(DateTime.Now.Millisecond);

            for (int i = Instance.Size; i >= 2; i--)
            {
                int temp = solution[i - 1];
                int spot = r.Next() % i;
                solution[i - 1] = solution[spot];
                solution[spot] = temp;
            }

            //for (int i = 0; i < solution.Length; i++)
            //   Console.Write("{0} ", solution[i]);
            //Console.WriteLine();

            return solution;
        }

        private int[] GenerateGreedyHeuristicSolution()
        {
            var solution = new int[InstanceSize];
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

                for (int i = 0; i < InstanceSize * InstanceSize; i++)
                {
                    if ((i % InstanceSize != i / InstanceSize) && Instance.DistanceMatrix[i] > highest && (solution[i % InstanceSize] == -1) &&
                        (solution[i / InstanceSize] == -1))
                    {
                        highest = Instance.DistanceMatrix[i];
                        highestPos = i;
                    }
                    if ((i % InstanceSize != i / InstanceSize) && (Instance.CostMatrix[i]) < lowest && (!solution.Contains(i % InstanceSize) && (!solution.Contains(i / InstanceSize))))
                    {
                        lowest = Instance.CostMatrix[i];
                        lowestPos = i;
                    }

                }

                if (highestPos == -1)
                    break;

                solution[highestPos / InstanceSize] = lowestPos / InstanceSize;
                solution[highestPos % InstanceSize] = lowestPos % InstanceSize;
            }

            if (InstanceSize % 2 == 1)
            {
                int index = Array.FindIndex(solution, 0, x => x == -1);
                solution[index] = (InstanceSize) * ((InstanceSize - 1) / 2) - solution.Sum() - 1;
            }

            return solution;
        }

        private int[] GenerateGreedyAntiHeuristicSolution()
        {
            var solution = new int[InstanceSize];
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

                for (int i = 0; i < InstanceSize * InstanceSize; i++)
                {
                    if ((i % InstanceSize != i / InstanceSize) && Instance.DistanceMatrix[i] > highestDistance && (solution[i % InstanceSize] == -1) &&
                        (solution[i / InstanceSize] == -1))
                    {
                        highestDistance = Instance.DistanceMatrix[i];
                        highestDistancePos = i;
                    }
                    if ((i % InstanceSize != i / InstanceSize) && (Instance.CostMatrix[i]) > highestCost && (!solution.Contains(i % InstanceSize) && (!solution.Contains(i / InstanceSize))))
                    {
                        highestCost = Instance.CostMatrix[i];
                        highestCostPos = i;
                    }

                }

                if (highestDistancePos == -1)
                    break;

                solution[highestDistancePos / InstanceSize] = highestCostPos / InstanceSize;
                solution[highestDistancePos % InstanceSize] = highestCostPos % InstanceSize;
            }

            if (InstanceSize % 2 == 1)
            {
                int index = Array.FindIndex(solution, 0, x => x == -1);
                solution[index] = (InstanceSize) * ((InstanceSize - 1) / 2) - solution.Sum() - 1;
            }

            return solution;
        }
    }
}