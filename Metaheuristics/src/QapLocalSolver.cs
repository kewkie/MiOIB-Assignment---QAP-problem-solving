using System;
using System.Linq;

namespace Metaheuristics
{
    delegate int[] GenerateInitialSolution();
    delegate int[] GenerateNeighbour(int number);
    internal delegate int[] SolveInstance(Neighbourhood neighbourhood);

    class QapLocalSolver : QapSolver
    {
        private LocalSearchType _searchType;
        private SolveInstance _solveInstance;

        

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

        public QapLocalSolver(QapInstance instance) : base(instance)
        {
        }

        public override int[] Solve()
        {
            int[] initialSolution = _initialSolutionGenerator();
            var perm = new Neighbourhood(initialSolution, NeighbourhoodType);
            return _solveInstance(perm);
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
                //Console.WriteLine("Hood no #{0}: {1}", iterations++, bestScore);
            }
            return bestHood;
        }
    }
}
