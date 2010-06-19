using System;
using System.Collections;
using System.Collections.Generic;

namespace Metaheuristics
{
    class TwoSwapRandomEnumerator : IEnumerator<Neighbour>
    {
        private Neighbour _current;
        private readonly int[] _base;
        private readonly Random _random = new Random((int)DateTime.Now.Ticks);
        private readonly SwapIndexPair[] _randomSwapPairs;
        private int _currentPair;
        
        public TwoSwapRandomEnumerator(int[] @base)
        {
            _base = @base;
            _current = new Neighbour
            {
                Solution = new int[_base.Length],
                SwappedIndices = new int[2],
                SwappedValues = new int[2]
            };
            _randomSwapPairs = GetRandomizedPairs(_base.Length);
            Reset();
        }
        
        public void Dispose()
        {
            GC.Collect();
        }

        public bool MoveNext()
        {
            if (_currentPair == _randomSwapPairs.Length)
                return false;

            _current = new Neighbour
            {
                Solution = new int[_base.Length],
                SwappedIndices = new int[2],
                SwappedValues = new int[2]
            };
            Array.Copy(_base, _current.Solution, _base.Length);

            _current.SwappedIndices[0] = _randomSwapPairs[_currentPair].First;
            _current.SwappedIndices[1] = _randomSwapPairs[_currentPair].Second;
            _current.SwappedValues[0] = _current.Solution[_randomSwapPairs[_currentPair].First];
            _current.SwappedValues[1] = _current.Solution[_randomSwapPairs[_currentPair].Second];

            Swap(_randomSwapPairs[_currentPair].First, _randomSwapPairs[_currentPair].Second);
            _currentPair++;
            return true;
        }

        public void Reset()
        {
            Array.Copy(_base, _current.Solution, _base.Length);
            _currentPair = 0;
        }

        public Neighbour Current
        {
            get { return _current; }
        }

        object IEnumerator.Current
        {
            get { return Current; }
        }

        private void Swap(int first, int second)
        {
            int swap = _current.Solution[first];
            _current.Solution[first] = _current.Solution[second];
            _current.Solution[second] = swap;
        }

        private SwapIndexPair[] GetRandomizedPairs(int instanceSize)
        {
            var pairs = GeneratePairs(instanceSize);
            for(int i = pairs.Length-1; i >= 0; i--)
            {
                int toSwapWith = _random.Next(0, i);
                SwapPairs(pairs, i, toSwapWith);
            }
            return pairs;
        }

        private static void SwapPairs(SwapIndexPair[] pairs, int firstIndex, int secondIndex)
        {
            var temp = pairs[firstIndex];
            pairs[firstIndex] = pairs[secondIndex];
            pairs[secondIndex] = temp;
        }

        private static SwapIndexPair[] GeneratePairs(int instanceSize)
        {
            int numOfPairs = ((instanceSize*instanceSize) - instanceSize)/2; //number of pairs generated
            int counter = 0;
            var pairs = new SwapIndexPair[numOfPairs];
            for(int i = 0; i < instanceSize-1; i++)
            {
                for(int j = i+1; j < instanceSize; j++)
                {
                    pairs[counter] = new SwapIndexPair{First = i, Second = j};
                    counter++;
                }
            }
            return pairs;
        }

        struct SwapIndexPair
        {
            public int First { get; set; }
            public int Second { get; set; }
        }
    }
}