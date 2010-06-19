using System;

namespace Metaheuristics
{
    class QapSASolver : QapSolver
    {
        private readonly Random _random = new Random();

        public int MaxIterations { get; set; }
        public int GoodEnoughScore { get; set; }
        public int MaxIterationsWithoutImprovement { get; set; }

        public QapSASolver()
        {
            MaxIterations = int.MaxValue;
        }

        public override int[] Solve(QapInstance instance)
        {
            var currentSolution = _initialSolutionGenerator(instance);
            var bestSolution = new int[currentSolution.Length];
            Array.Copy(currentSolution, bestSolution, currentSolution.Length);
            int k = 1;
            int currentScore = Evaluate(currentSolution, instance);
            int bestScore = currentScore;
            int noImprovement = 0;
            while(!StopConditionMet(currentScore, k, noImprovement))
            {
                var neighbourSolution = RandomNeighbour(currentSolution);
                var neighbourScore = Evaluate(neighbourSolution, instance);
                if(neighbourScore < bestScore)
                {
                    Array.Copy(neighbourSolution, bestSolution, neighbourSolution.Length);
                    bestScore = neighbourScore;
                }
                if(Probability(currentScore, neighbourScore, Temperature(k/(double)MaxIterations)) > _random.NextDouble())
                {
                    Array.Copy(neighbourSolution, currentSolution, neighbourSolution.Length);
                    currentScore = neighbourScore;
                    noImprovement = 0;
                }
                else
                {
                    noImprovement++;
                }
                k++;
            }
            return bestSolution;
        }

        public override void Reset()
        {
        }

        private double Probability(int currentScore, int receivedScore, double temperature)
        {
            if(receivedScore > currentScore)
                return temperature/Math.Log(MaxIterations);
            return 1;
        }

        private static double Temperature(double progress)
        {
            return -Math.Log(progress);
        }

        private int[] RandomNeighbour(int[] solution)
        {
            var neighbour = new int[solution.Length];
            Array.Copy(solution, neighbour, solution.Length);
            var swapIndex = _random.Next(solution.Length - 1);
            var temp = neighbour[swapIndex];
            neighbour[swapIndex] = neighbour[swapIndex + 1];
            neighbour[swapIndex + 1] = temp;
            return neighbour;
        }

        private bool StopConditionMet(int currentScore, int iteration, int noImprovement)
        {
            if(iteration >= MaxIterations)
                return true;
            if(currentScore <= GoodEnoughScore)
                return true;
            if(noImprovement >= MaxIterationsWithoutImprovement)
                return true;
            return false;
        }
    }
}