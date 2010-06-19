using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Timers;

namespace Metaheuristics
{
    static class Experiment
    {   
        public static Dictionary<TimeSpan,double> SolveWithTimeLimit(TimeSpan limit, TimeSpan grain, QapInstance instance, QapSolver solver)
        {
            int[] counter = {0};
            int grainTimeElapsedCount = 0;
            var resultsInTime = new Dictionary<TimeSpan, double>();
            var bestResult = new int[instance.Size];
            for (int i = 0; i < bestResult.Length; i++ )
            {
                bestResult[i] = i;
            }
            var globalTimer = new Timer(limit.TotalMilliseconds);
            var grainTimer = new Timer(grain.TotalMilliseconds) {AutoReset = true};
            var timerElapsed = false;
            grainTimer.Elapsed += delegate
                                      {
                                          var bestScore = QapSolver.Evaluate(bestResult, instance);
                                          grainTimeElapsedCount++;
                                          resultsInTime.Add(new TimeSpan(0, 0, grainTimeElapsedCount * grain.Seconds), 
                                                            Quality(bestScore, instance));
                                          Console.WriteLine("Best Score: {0}, Count: {1}, Quality: {2}, counter: {3}", 
                                              ((counter[0]==0)?(-1):(bestScore)), 
                                              resultsInTime.Count, 
                                              ((counter[0]==0)?(0):(Quality(bestScore, instance)))
                                              , counter[0] );
                                      };
            globalTimer.Elapsed += delegate { timerElapsed = true; grainTimer.Stop();};
            int[] result;
           
            while(!timerElapsed)
            {
               globalTimer.Start();
               grainTimer.Start();
               result = solver.Solve(instance);
               counter[0]++;
               if(QapSolver.Evaluate(result, instance) < QapSolver.Evaluate(bestResult, instance))
               {
                   lock (bestResult)
                   {
                       Array.Copy(result, bestResult, result.Length);
                   } 
               }
            }
            return resultsInTime;
        }

        private static double Quality(int score, QapInstance instance)
        {
            return 1 - ((score - instance.OptimalScore)/(double) instance.OptimalScore);
        }

        public static IList<TimeSpan> TimeToSolveWithCountLimit(int count, QapSolver solver, QapInstance instance)
        {
            var stopwatch = new Stopwatch();
            var times = new List<TimeSpan>();
            for(int i =0; i < count; i++)
            {
                stopwatch.Start();
                solver.Solve(instance);
                stopwatch.Stop();
                times.Add(stopwatch.Elapsed);
                stopwatch.Reset();
            }

            return times;
        }

        public static IList<double> ScoreWithCountLimit(int count, QapSolver solver, QapInstance instance)
        {
            var scoreList = new List<double>();
            for (int i = 0; i < count; i++)
            {
                var result = solver.Solve(instance);
                scoreList.Add(Quality(QapSolver.Evaluate(result, instance), instance));
            }
            return scoreList;
        }

        public static void SaveResultsAsCsv(string algorithm, IDictionary<string, IList<double>> results, string filename)
        {
            var stream = new FileStream(filename + ".csv", FileMode.Create, FileAccess.Write, FileShare.Read);
            var writer = new StreamWriter(stream);
            var line = "alg;inst;score";
            writer.WriteLine(line);
            foreach (var result in results)
            {
                foreach (var value in result.Value)
                {
                    line = algorithm + ";" + result.Key + ";" + value;
                    writer.WriteLine(line);
                }
            }
            writer.Close();
        }

        public static void SaveResultsAsCsv(string algorithm, IDictionary<string, IList<TimeSpan>> results, string filename)
        {
            var stream = new FileStream(filename + ".csv", FileMode.Create, FileAccess.Write, FileShare.Read);
            var writer = new StreamWriter(stream);
            var line = "alg;inst;score";
            writer.WriteLine(line);
            foreach (var result in results)
            {
                foreach (var value in result.Value)
                {
                    line = algorithm + ";" + result.Key + ";" + value.TotalMilliseconds;
                    writer.WriteLine(line);
                }
            }
            writer.Close();
        }

        public static void SaveResultsAsCsv(string algorithm, IDictionary<string, IDictionary<TimeSpan, double>> results, string filename)
        {
            var stream = new FileStream(filename + ".csv", FileMode.Create, FileAccess.Write, FileShare.Read);
            var writer = new StreamWriter(stream);
            var line = "alg;inst;time;score";
            writer.WriteLine(line);
            foreach (var result in results)
            {
                foreach (var value in result.Value)
                {
                    line = algorithm + ";" + result.Key + ";" + value.Key.TotalSeconds + ";" + value.Value;
                    writer.WriteLine(line);
                }
            }
            writer.Close();
        }
    }
}