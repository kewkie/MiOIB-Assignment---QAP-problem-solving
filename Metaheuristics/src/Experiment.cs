using System;
using System.Collections.Generic;
using System.Timers;

namespace Metaheuristics
{
    class Experiment
    {
        private readonly QapSolver _solver;
        
        public Experiment(QapSolver solver)
        {
            _solver = solver;
        }

        public List<int> SolveWithTimeLimit(TimeSpan limit, TimeSpan grain)
        {
            var counter = 0;
            var compTimes = new List<int>();
            var bestResult = new int[_solver.InstanceSize];
            for (int i = 0; i < bestResult.Length; i++ )
            {
                bestResult[i] = i;
            }
            var globalTimer = new Timer(limit.TotalMilliseconds);
            var grainTimer = new Timer(grain.TotalMilliseconds) {AutoReset = true};
            var timerElapsed = false;
            grainTimer.Elapsed += delegate(object sender, ElapsedEventArgs e)
                                      {
                                          compTimes.Add(_solver.Evaluate(bestResult));
                                          Console.WriteLine("Best Score: {0}, Count: {1}, at: {2}, counter: {3}", 
                                              _solver.Evaluate(bestResult), compTimes.Count, e.SignalTime.Millisecond, counter );
                                      };
            globalTimer.Elapsed += delegate { timerElapsed = true; grainTimer.Stop();};
            int[] result;
           
            while(!timerElapsed)
            {
               globalTimer.Start();
               grainTimer.Start();
               result = _solver.Solve();
               counter++;
               if(_solver.Evaluate(result) < _solver.Evaluate(bestResult))
               {
                   lock (bestResult)
                   {
                       Array.Copy(result, bestResult, result.Length);
                   } 
               }
            }
            return compTimes;
        }

        public TimeSpan SolveWithCountLimit(int count)
        {
            throw new NotImplementedException();
        }
    }
}