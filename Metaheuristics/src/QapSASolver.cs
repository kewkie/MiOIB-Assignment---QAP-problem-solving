using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Metaheuristics
{
    class QapSASolver : QapSolver
    {
        private int _currentScore;
        
        public QapSASolver(QapInstance instance) : base(instance)
        {
        }

        public int MaxIterations { get; set; }
        public int GoodEnoughScore { get; set; }

        public override int[] Solve()
        {
            int[] currentSolution = _initialSolutionGenerator();
            int k = 0;
            while(k < MaxIterations && Evaluate(currentSolution) > GoodEnoughScore)
            {
                
            }
        }

        private double Probability(int[] current, int[] received, double temperature)
        {
            throw new NotImplementedException();
        }

        private double Temperature(int iteration)
        {
            throw new NotImplementedException();
        }

        private int[] RandomNeighbour(int[] solution)
        {
            throw new NotImplementedException();
        }
    }
}