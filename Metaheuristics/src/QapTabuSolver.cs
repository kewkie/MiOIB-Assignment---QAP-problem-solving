using System;
using System.Collections.Generic;
using System.Linq;

namespace Metaheuristics
{
    class QapTabuSolver : QapSolver
    {
        private int[,] _tabooMatrix;

        public int TabooSize { get; set; }

        public int MaxIterations { get; set; }

        public int MaxIterationsWithoutImprovement { get; set; }

        public int MasterListSize { get; set; }

        public double TresholdPercentage { get; set; }

        private int[] Solve(Neighbourhood hood, QapInstance instance)
        {
            var bestScore = int.MaxValue;
            var bestHood = new int[hood.Base.Length];
            var treshold = int.MaxValue;
            var iterations = 0;
            var iterationsWithoutImprovement = 0;
            var listUsed = false;
            var stopConditionMet = false;
            _tabooMatrix = new int[instance.Size, instance.Size];
            Stack<Neighbour> masterList = RebuildMasterList(hood, instance);
            while (!stopConditionMet)
            {
                if (masterList.Count == 0)
                {
                    if(!listUsed)
                    {
                        masterList = RebuildMasterList(hood, instance);
                        var neighbour = masterList.Pop();
                        Array.Copy(neighbour.Solution, hood.Base, hood.Base.Length);
                    }
                    masterList = RebuildMasterList(hood, instance);
                    treshold = Convert.ToInt32(bestScore + bestScore*TresholdPercentage);
                    listUsed = false;
                }
                else
                {
                    var neighbour = masterList.Pop();
                    neighbour = FromMove(neighbour, hood.Base);
                    var currentScore = Evaluate(neighbour.Solution, instance);
                    if (currentScore < treshold)
                    {
                        DecrementTaboo();
                        listUsed = true;
                        if (!IsTaboo(neighbour) || IsAspiring(currentScore, bestScore))
                            // (q && p) || !q <=> !(q && !p) <=> !q || p
                        {
                            Array.Copy(neighbour.Solution, hood.Base, hood.Base.Length);
                            
                            if(neighbour.SwappedValues[0] > neighbour.SwappedValues[1])
                                _tabooMatrix[neighbour.SwappedValues[1], neighbour.SwappedValues[0]] = TabooSize;
                            else
                                _tabooMatrix[neighbour.SwappedValues[0], neighbour.SwappedValues[1]] = TabooSize;   
                            
                            
                            if (currentScore < bestScore)
                            {
                                Array.Copy(neighbour.Solution, bestHood, bestHood.Length);
                                bestScore = currentScore;
                                iterationsWithoutImprovement = 0;
                            }
                            else
                            {
                                iterationsWithoutImprovement++;
                            }
                        }
                    }
                    else
                    {
                        masterList.Clear();
                    }
                }
                iterations++;
                stopConditionMet = CheckStopCondition(iterations, iterationsWithoutImprovement);
                //Console.WriteLine("Hood no #{0}: {1}", iterations++, bestScore);
            }
            //Console.WriteLine("rebuild ratio: {0}", rebuilt/(double)MaxIterations);
            return bestHood;
        }

        private Stack<Neighbour> RebuildMasterList(IEnumerable<Neighbour> hood, QapInstance instance)
        {
            var masterList = new List<Neighbour>();
            foreach (var neighbour in hood)
            {
                masterList.Add(neighbour);
            }

            var topCandidates = masterList
                .OrderBy(x => Evaluate(x.Solution, instance))
                .Take(MasterListSize)
                .Reverse();

            var stack = new Stack<Neighbour>();
            foreach (var candidate in topCandidates)
            {
                stack.Push(candidate);
            }
            return stack;
        }



        private void DecrementTaboo()
        {
            for (int i = 0; i < _tabooMatrix.GetLength(0); i++)
                for (int j = 0; j < _tabooMatrix.GetLength(1); j++)
                    if(_tabooMatrix[i,j] != 0)
                        _tabooMatrix[i, j]--;
        }

        private bool IsTaboo(Neighbour neighbour)
        {
            int firstValue;
            int secondValue;
            if(neighbour.SwappedValues[0] > neighbour.SwappedValues[1])
            {
                firstValue = neighbour.SwappedValues[1];
                secondValue = neighbour.SwappedValues[0];
            }
            else
            {
                firstValue = neighbour.SwappedValues[0];
                secondValue = neighbour.SwappedValues[1];
            }
            if (_tabooMatrix[firstValue, secondValue] != 0)
                return true;
            return false;
        }

        private static bool IsAspiring(int currentScore, int bestScore)
        {
            if(currentScore < bestScore)
                return true;
            return false;
        }

        private static Neighbour FromMove(Neighbour move, int[] baseSolution)
        {
            Array.Copy(baseSolution, move.Solution, baseSolution.Length);
            var i1 = Array.IndexOf(move.Solution, move.SwappedValues[0]);
            var i2 = Array.IndexOf(move.Solution, move.SwappedValues[1]);
            Swap(i1,i2, move.Solution);
            return move;
        }

        private static void Swap(int i1, int i2, int[] array)
        {
            var temp = array[i1];
            array[i1] = array[i2];
            array[i2] = temp;
        }

        private bool CheckStopCondition(int iterations, int iterationsWithoutImprovement)
        {
            if(MaxIterations > 0)
            {
                if(iterations >= MaxIterations)
                    return true;
                return false;
            }
            if (MaxIterationsWithoutImprovement > 0)
            {
                if (iterationsWithoutImprovement >= MaxIterationsWithoutImprovement)
                    return true;
                return false;
            }
            throw new InvalidOperationException("Stop condition not set.");
        }

        public override int[] Solve(QapInstance instance)
        {
            int[] initialSolution = _initialSolutionGenerator(instance);
            var perm = new Neighbourhood(initialSolution, NeighbourhoodType.TwoSwap);
            return Solve(perm, instance);
        }

        public override void Reset()
        {
            Array.Clear(_tabooMatrix, 0, _tabooMatrix.Length);
        }
    }
}