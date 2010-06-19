using System;

namespace Metaheuristics
{
    internal delegate int[] SolveInstance(Neighbourhood neighbourhood, QapInstance instance);

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

        public override int[] Solve(QapInstance instance)
        {
            int[] initialSolution = _initialSolutionGenerator(instance);
            var perm = new Neighbourhood(initialSolution, NeighbourhoodType);
            return _solveInstance(perm, instance);
        }

        public override void Reset()
        {
            throw new NotImplementedException();
        }

        private static int[] SolveGreedy(Neighbourhood hood, QapInstance instance)
        {
            var bestScore = int.MaxValue;
            var solveFinished = false;
            while(!solveFinished)
            {
                solveFinished = true;
                foreach (var neighbourhood in hood)
                {
                    var currentScore = Evaluate(neighbourhood.Solution, instance);
                    if (currentScore < bestScore)
                    {
                        bestScore = currentScore;
                        solveFinished = false;
                        hood.Base = neighbourhood.Solution;
                        break;
                    }
                }
            }
            return hood.Base;
        }

        private static int[] SolveSteepest(Neighbourhood hood, QapInstance instance)
        {
            var bestScore = int.MaxValue;
            var solveFinished = false;
            var bestHood = new int[hood.Base.Length];
            while (!solveFinished)
            {
                solveFinished = true;
                foreach (var neighbourhood in hood)
                {
                    var currentScore = Evaluate(neighbourhood.Solution, instance);
                    if (currentScore < bestScore)
                    {
                        bestScore = currentScore;
                        solveFinished = false;
                        Array.Copy(neighbourhood.Solution, bestHood, bestHood.Length);
                    }
                }
                Array.Copy(bestHood, hood.Base, bestHood.Length);
                //Console.WriteLine("Hood no #{0}: {1}", iterations++, bestScore);
            }
            return bestHood;
        }
    }
}
