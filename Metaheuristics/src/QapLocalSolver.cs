using System;

namespace Metaheuristics
{
    delegate int[] GenerateInitialSolution();
    delegate int[] GenerateNeighbour(int number);
    internal delegate int[] SolveInstance(Neighbourhood neighbourhood);

    class QapLocalSolver
    {
        private readonly QapInstance _instance;
        private InitialSolution _initialSolution;
        private GenerateInitialSolution _initialSolutionGenerator;
        private LocalSearchType _searchType;
        private SolveInstance _solveInstance;

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
                    default: 
                        _initialSolutionGenerator = GenerateRandomSolution; 
                        break;
                }

            }
        }

        public LocalSearchType SearchType
        {
            get
            {
                return _searchType;
            }
            set
            {
                _searchType = value;
                switch (value)
                {
                    case LocalSearchType.Greedy:
                        _solveInstance = SolveGreedy;
                        break;
                    case LocalSearchType.Steepest:
                        _solveInstance = SolveSteepest;
                        break;
                    default:
                        _solveInstance = SolveGreedy;
                        break;
                }
            }
        }

        public NeighbourhoodType NeighbourhoodType { get; set; }
        public int Seed { get; set; }


        public QapLocalSolver(QapInstance instance)
        {
            _instance = instance;
        }

        public int[] Solve()
        {
            int[] initialSolution = _initialSolutionGenerator();
            var perm = new Neighbourhood(initialSolution, NeighbourhoodType);
            return _solveInstance(perm);
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
            //Console.WriteLine("Evaluation = {0}", totalCost);
            return totalCost;
        }

        public int[] GenerateRandomSolution()
        {
            int[] solution = new int[_instance.Size];

            for (int i = 0; i < _instance.Size ; i++)
			    solution[i] = i;
            
            Random r = new Random(Seed);
            
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
                {
                }
            }
             

            return solution;
        }

        private int[] SolveGreedy(Neighbourhood hood)
        {
            var bestScore = int.MaxValue;
            var solveFinished = false;
            while(!solveFinished)
            {
                solveFinished = true;
                foreach (var neighbourhood in hood)
                {
                    var currentScore = Evaluate(neighbourhood);
                    if (currentScore < bestScore)
                    {
                        bestScore = currentScore;
                        solveFinished = false;
                        hood.Base = neighbourhood;
                        break;
                    }
                }
            }
            return hood.Base;
        }

        private int[] SolveSteepest(Neighbourhood hood)
        {
            var bestScore = int.MaxValue;
            var solveFinished = false;
            var bestHood = new int[hood.Base.Length];
            var iterations = 0;
            while (!solveFinished)
            {
                solveFinished = true;
                foreach (var neighbourhood in hood)
                {
                    var currentScore = Evaluate(neighbourhood);
                    if (currentScore < bestScore)
                    {
                        bestScore = currentScore;
                        solveFinished = false;
                        Array.Copy(neighbourhood,bestHood, bestHood.Length);
                    }
                }
                Array.Copy(bestHood, hood.Base, bestHood.Length);
                Console.WriteLine("Hood no #{0}: {1}", iterations++, bestScore);
            }
            return bestHood;
        }
    }
}
