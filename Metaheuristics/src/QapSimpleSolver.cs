using System;

namespace Metaheuristics
{
    class QapSimpleSolver : QapSolver
    {
        public override int[] Solve(QapInstance instance)
        {
            return _initialSolutionGenerator(instance);
        }

        public override void Reset()
        {
            throw new NotImplementedException();
        }
    }
}
