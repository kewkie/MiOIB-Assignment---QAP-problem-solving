using System;
using System.Collections.Generic;
using System.IO;

namespace Metaheuristics
{
    internal class Program
    {
        private static QapSASolver _sa;
        private static QapTabuSolver _ts;
        private static QapLocalSolver _steepestTwoSwapAntiHeuristic;
        private static QapLocalSolver _steepestTwoSwapHeuristic;
        private static QapLocalSolver _greedyTwoSwapAntiHeuristic;
        private static QapLocalSolver _greedyTwoSwapHeuristic;
        private static QapLocalSolver _steepestTwoSwapRandom;
        private static QapLocalSolver _greedyTwoSwapRandom;
        private static QapLocalSolver _steepestThreeSwapAntiheuristic;
        private static QapLocalSolver _steepestThreeSwapHeuristic;
        private static QapLocalSolver _steepestThreeSwapRandom;
        private static QapLocalSolver _greedyThreeSwapAntiheur;
        private static QapLocalSolver _greedyThreeSwapHeuristic;
        private static QapLocalSolver _greedyRandomTwoSwapRandom;
        private static QapLocalSolver _greedyRandomThreeSwapRandom;
        private static QapLocalSolver _greedyThreeSwapRandom;
        private static List<QapInstance> _instances;

        private static void Main()
        {
            try
            {
                #region Field init
                _instances = new List<QapInstance>();

                var scr20 = InstanceIO.ReadInstance("res/scr20");
                scr20.PrintInstance(PrintInstanceMode.Sparse);
                _instances.Add(scr20);
                var chr20b = InstanceIO.ReadInstance("res/chr20b");
                chr20b.PrintInstance(PrintInstanceMode.Sparse);
                _instances.Add(chr20b);
                var bur26g = InstanceIO.ReadInstance("res/bur26g");
                _instances.Add(bur26g);
                bur26g.PrintInstance(PrintInstanceMode.Sparse);
                var nug30 = InstanceIO.ReadInstance("res/nug30");
                nug30.PrintInstance(PrintInstanceMode.Sparse);
                _instances.Add(nug30);
                var kra30a = InstanceIO.ReadInstance("res/kra30a");
                _instances.Add(kra30a);
                kra30a.PrintInstance(PrintInstanceMode.Sparse);
                var esc32f = InstanceIO.ReadInstance("res/esc32f");
                esc32f.PrintInstance(PrintInstanceMode.Sparse);
                _instances.Add(esc32f);
                var ste36c = InstanceIO.ReadInstance("res/ste36c");
                ste36c.PrintInstance(PrintInstanceMode.Sparse);
                _instances.Add(ste36c);
                var tai40b = InstanceIO.ReadInstance("res/tai40b");
                tai40b.PrintInstance(PrintInstanceMode.Sparse);
                _instances.Add(tai40b);
                var lipa50b = InstanceIO.ReadInstance("res/lipa50b");
                _instances.Add(lipa50b);
                lipa50b.PrintInstance(PrintInstanceMode.Sparse);
                //var tai64c = InstanceIO.ReadInstance("res/tai64c");
                //instances.Add(tai64c);
                //tai64c.PrintInstance(PrintInstanceMode.Sparse);
                //var tai80a = InstanceIO.ReadInstance("res/tai80a");
                //instances.Add(tai80a);
                //tai80a.PrintInstance(PrintInstanceMode.Sparse);
                //var sko81 = InstanceIO.ReadInstance("res/sko81");
                //instances.Add(sko81);
                //sko81.PrintInstance(PrintInstanceMode.Sparse);
                //var wil100 = InstanceIO.ReadInstance("res/wil100");
                //instances.Add(wil100);
                //wil100.PrintInstance(PrintInstanceMode.Sparse);
                //var sko100c = InstanceIO.ReadInstance("res/sko100c");
                //sko100c.PrintInstance(PrintInstanceMode.Sparse);
                //instances.Add(sko100c);
                ////var esc128 = InstanceIO.ReadInstance("res/esc128");
                ////instances.Add(esc128);
                ////esc128.PrintInstance(PrintInstanceMode.Sparse);
                //var tho150 = InstanceIO.ReadInstance("res/tho150");
                //instances.Add(tho150);
                //tho150.PrintInstance(PrintInstanceMode.Sparse);

                _greedyThreeSwapRandom = new QapLocalSolver
                                             {
                                                 InitialSolution = InitialSolution.Random,
                                                 NeighbourhoodType = NeighbourhoodType.ThreeSwap,
                                                 SearchType = LocalSearchType.Greedy
                                             };

                _greedyRandomThreeSwapRandom = new QapLocalSolver
                                                   {
                                                       InitialSolution = InitialSolution.Random,
                                                       NeighbourhoodType = NeighbourhoodType.ThreeSwapRandom,
                                                       SearchType = LocalSearchType.Greedy
                                                   };

                _greedyRandomTwoSwapRandom = new QapLocalSolver
                                                 {
                                                     InitialSolution = InitialSolution.Random,
                                                     NeighbourhoodType = NeighbourhoodType.TwoSwapRandom,
                                                     SearchType = LocalSearchType.Greedy
                                                 };

                _greedyThreeSwapHeuristic = new QapLocalSolver
                                                {
                                                    InitialSolution = InitialSolution.Heuristic,
                                                    NeighbourhoodType = NeighbourhoodType.ThreeSwap,
                                                    SearchType = LocalSearchType.Greedy
                                                };
                _greedyThreeSwapAntiheur = new QapLocalSolver
                                               {
                                                   InitialSolution = InitialSolution.AntiHeuristic,
                                                   NeighbourhoodType = NeighbourhoodType.ThreeSwap,
                                                   SearchType = LocalSearchType.Greedy
                                               };

                _steepestThreeSwapRandom = new QapLocalSolver
                                               {
                                                   InitialSolution = InitialSolution.Random,
                                                   NeighbourhoodType = NeighbourhoodType.ThreeSwap,
                                                   SearchType = LocalSearchType.Steepest
                                               };

                _steepestThreeSwapHeuristic = new QapLocalSolver
                                                  {
                                                      InitialSolution = InitialSolution.Heuristic,
                                                      NeighbourhoodType = NeighbourhoodType.ThreeSwap,
                                                      SearchType = LocalSearchType.Steepest
                                                  };
                _steepestThreeSwapAntiheuristic = new QapLocalSolver
                                                      {
                                                          InitialSolution = InitialSolution.AntiHeuristic,
                                                          NeighbourhoodType = NeighbourhoodType.ThreeSwap,
                                                          SearchType = LocalSearchType.Steepest
                                                      };

                _greedyTwoSwapRandom = new QapLocalSolver
                                           {
                                               InitialSolution = InitialSolution.Random,
                                               NeighbourhoodType = NeighbourhoodType.TwoSwap,
                                               SearchType = LocalSearchType.Greedy
                                           };

                _steepestTwoSwapRandom = new QapLocalSolver
                                             {
                                                 InitialSolution = InitialSolution.Random,
                                                 NeighbourhoodType = NeighbourhoodType.TwoSwap,
                                                 SearchType = LocalSearchType.Steepest
                                             };

                _greedyTwoSwapHeuristic = new QapLocalSolver
                                              {
                                                  InitialSolution = InitialSolution.Heuristic,
                                                  NeighbourhoodType = NeighbourhoodType.TwoSwap,
                                                  SearchType = LocalSearchType.Greedy
                                              };

                _greedyTwoSwapAntiHeuristic = new QapLocalSolver
                                                  {
                                                      InitialSolution = InitialSolution.AntiHeuristic,
                                                      NeighbourhoodType = NeighbourhoodType.TwoSwap,
                                                      SearchType = LocalSearchType.Greedy
                                                  };

                _steepestTwoSwapHeuristic = new QapLocalSolver
                                                {
                                                    InitialSolution = InitialSolution.Heuristic,
                                                    NeighbourhoodType = NeighbourhoodType.TwoSwap,
                                                    SearchType = LocalSearchType.Steepest
                                                };

                _steepestTwoSwapAntiHeuristic = new QapLocalSolver
                                                    {
                                                        InitialSolution = InitialSolution.AntiHeuristic,
                                                        NeighbourhoodType = NeighbourhoodType.TwoSwap,
                                                        SearchType = LocalSearchType.Steepest
                                                    };

                _ts = new QapTabuSolver
                          {
                              InitialSolution = InitialSolution.Random,
                              MasterListSize = 50,
                              MaxIterationsWithoutImprovement = 200,
                              TabooSize = 30,
                              TresholdPercentage = 0.03
                          };

                _sa = new QapSASolver
                          {
                              InitialSolution = InitialSolution.Random,
                              MaxIterations = 100000,
                              MaxIterationsWithoutImprovement = 200
                          };

                #endregion

                RandomTwoSwapTest();
            }
            catch (FileNotFoundException fnfe)
            {
                Console.WriteLine(fnfe.Message);
            }
            Console.ReadLine();
        }

        private void AvgScoreExperiments()
        {
            var greedyThreeRandomResults = new Dictionary<string, IList<double>>();
            var greedyThreeHeurResults = new Dictionary<string, IList<double>>();
            var greedyThreeAntiResults = new Dictionary<string, IList<double>>();
            var steepestThreeRandom = new Dictionary<string, IList<double>>();
            var steepestThreeHeur = new Dictionary<string, IList<double>>();
            var steepestThreeAnti = new Dictionary<string, IList<double>>();

            var greedyTwoHeurResults = new Dictionary<string, IList<double>>();
            var greedyTwoAntiHeurResults = new Dictionary<string, IList<double>>();
            var steepestTwoHeurResults = new Dictionary<string, IList<double>>();
            var steepestTwoAntiHeurResults = new Dictionary<string, IList<double>>();

            foreach (var instance in _instances)
            {
                greedyThreeRandomResults.Add(instance.Name, Experiment.ScoreWithCountLimit(10, _greedyThreeSwapRandom, instance));
                Console.WriteLine("greedyThreeRandom computed for inst {0}", instance.Name);

                greedyThreeHeurResults.Add(instance.Name, Experiment.ScoreWithCountLimit(10, _greedyThreeSwapHeuristic, instance));
                Console.WriteLine("greedyThreeHeuristic computed for inst {0}", instance.Name);

                greedyThreeAntiResults.Add(instance.Name, Experiment.ScoreWithCountLimit(10, _greedyThreeSwapAntiheur, instance));
                Console.WriteLine("greedyThreeAnti computed for inst {0}", instance.Name);

                steepestThreeRandom.Add(instance.Name, Experiment.ScoreWithCountLimit(10, _steepestThreeSwapRandom, instance));
                Console.WriteLine("steepestThreeRandom computed for inst {0}", instance.Name);

                steepestThreeHeur.Add(instance.Name, Experiment.ScoreWithCountLimit(10, _steepestThreeSwapHeuristic, instance));
                Console.WriteLine("steepestThreeHeuristic computed for inst {0}", instance.Name);

                steepestThreeAnti.Add(instance.Name, Experiment.ScoreWithCountLimit(10, _steepestThreeSwapAntiheuristic, instance));
                Console.WriteLine("steepestThreeAnti computed for inst {0}", instance.Name);

                greedyTwoHeurResults.Add(instance.Name, Experiment.ScoreWithCountLimit(10, _greedyTwoSwapHeuristic, instance));
                Console.WriteLine("greedyTwoHeuristic computed for inst {0}", instance.Name);

                greedyTwoAntiHeurResults.Add(instance.Name, Experiment.ScoreWithCountLimit(10, _greedyTwoSwapAntiHeuristic, instance));
                Console.WriteLine("greedyTwoAnti computed for inst {0}", instance.Name);

                steepestTwoHeurResults.Add(instance.Name, Experiment.ScoreWithCountLimit(10, _steepestTwoSwapHeuristic, instance));
                Console.WriteLine("steepestTwoHeuristic computed for inst {0}", instance.Name);

                steepestTwoAntiHeurResults.Add(instance.Name, Experiment.ScoreWithCountLimit(10, _steepestTwoSwapAntiHeuristic, instance));
                Console.WriteLine("steepestTwoAnti computed for inst {0}", instance.Name);
            }

            Experiment.SaveResultsAsCsv("greedyThreeRandom", greedyThreeRandomResults, "greedy_three_random");
            Experiment.SaveResultsAsCsv("greedyThreeHeuristic", greedyThreeHeurResults, "greedy_three_heuristic");
            Experiment.SaveResultsAsCsv("greedyThreeAnti", greedyThreeAntiResults, "greedy_three_anti");
            Experiment.SaveResultsAsCsv("steepestThreeRandom", steepestThreeRandom, "steepest_three_random");
            Experiment.SaveResultsAsCsv("steepestThreeHeuristic", steepestThreeHeur, "steepest_three_heuristic");
            Experiment.SaveResultsAsCsv("steepestThreeAnti", steepestThreeAnti, "steepest_three_antiheuristic");
            Experiment.SaveResultsAsCsv("greedyTwoHeuristic", greedyTwoHeurResults, "greedy_two_heuristic");
            Experiment.SaveResultsAsCsv("greedyTwoAntiHeuristic", greedyTwoAntiHeurResults, "greedy_two_anti");
            Experiment.SaveResultsAsCsv("steepestTwoHeuristic", steepestTwoHeurResults, "steepest_two_heuristic");
            Experiment.SaveResultsAsCsv("steepestTwoAnti", steepestTwoAntiHeurResults, "steepest_two_anti");

            Console.WriteLine("Average score experiments saved.");
        }

        private void AvgTimeExperiment()
        {
            var greedyTwoSwapRandomTime = new Dictionary<string, IList<TimeSpan>>();
            var steepestTwoSwapRandomTime = new Dictionary<string, IList<TimeSpan>>();
            var tsTime = new Dictionary<string, IList<TimeSpan>>();
            var saTime = new Dictionary<string, IList<TimeSpan>>();

            foreach (var instance in _instances)
            {
                greedyTwoSwapRandomTime.Add(instance.Name, Experiment.TimeToSolveWithCountLimit(10, _greedyTwoSwapRandom, instance));
                steepestTwoSwapRandomTime.Add(instance.Name, Experiment.TimeToSolveWithCountLimit(10, _steepestTwoSwapRandom, instance));
                tsTime.Add(instance.Name, Experiment.TimeToSolveWithCountLimit(10, _ts, instance));
                saTime.Add(instance.Name, Experiment.TimeToSolveWithCountLimit(10, _sa, instance));
            }

            Experiment.SaveResultsAsCsv("greedyTwoRandom", greedyTwoSwapRandomTime, "greedy_two_random_times");
            Experiment.SaveResultsAsCsv("steepestTwoRandom", steepestTwoSwapRandomTime, "steepest_two_random_times");
            Experiment.SaveResultsAsCsv("ts", tsTime, "ts_times");
            Experiment.SaveResultsAsCsv("sa", saTime, "sa_times");

            Console.WriteLine("Times of algorithms saved.");
        }

        private void ImprovmentExperiment()
        {
            var greedyTwoSwapImprovements = new Dictionary<string, IDictionary<TimeSpan, double>>();
            var steepestTwoSwapImprovements = new Dictionary<string, IDictionary<TimeSpan, double>>();
            var tsImprovements = new Dictionary<string, IDictionary<TimeSpan, double>>();
            var saImprovements = new Dictionary<string, IDictionary<TimeSpan, double>>();

            foreach (var instance in _instances)
            {
                greedyTwoSwapImprovements.Add(instance.Name, Experiment.SolveWithTimeLimit(new TimeSpan(0, 10, 0), new TimeSpan(0, 0, 30), instance, _greedyTwoSwapRandom));
                steepestTwoSwapImprovements.Add(instance.Name, Experiment.SolveWithTimeLimit(new TimeSpan(0, 10, 0), new TimeSpan(0, 0, 30), instance, _steepestTwoSwapRandom));
                tsImprovements.Add(instance.Name, Experiment.SolveWithTimeLimit(new TimeSpan(0, 10, 0), new TimeSpan(0, 0, 30), instance, _ts));
                saImprovements.Add(instance.Name, Experiment.SolveWithTimeLimit(new TimeSpan(0, 10, 0), new TimeSpan(0, 0, 30), instance, _sa));
            }

            Experiment.SaveResultsAsCsv("greedyTwoRandom", greedyTwoSwapImprovements, "greedy_two_improv");
            Experiment.SaveResultsAsCsv("steepestTwoRandom", steepestTwoSwapImprovements, "steepest_two_improv");
            Experiment.SaveResultsAsCsv("ts", tsImprovements, "ts_improv");
            Experiment.SaveResultsAsCsv("sa", saImprovements, "sa_improv");

            Console.WriteLine("Improvments of algorithms saved");
        }

        private static void RandomTwoSwapTest()
        {
            var instance = InstanceIO.ReadInstance("res/lipa50b");
            instance.PrintInstance(PrintInstanceMode.Sparse);

            var results = Experiment.ScoreWithCountLimit(20, _greedyThreeSwapRandom
                , instance);
            foreach (var result in results)
            {
                Console.WriteLine(result);
            }
            Console.ReadLine();
        }
    }
}