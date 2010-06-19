using System;
using System.Collections;
using System.Collections.Generic;

namespace Metaheuristics
{
    class ThreeSwapRandomEnumerator : IEnumerator<Neighbour>
    {
        private Neighbour _current;
        private readonly int[] _base;
        private readonly Random _random = new Random((int)DateTime.Now.Ticks);
        private readonly SwapIndexTriple[] _randomSwapTriples;
        private int _currentTriple;
        
        public ThreeSwapRandomEnumerator(int[] @base)
        {
            _base = @base;
            _current = new Neighbour
            {
                Solution = new int[_base.Length],
                SwappedIndices = new int[3],
                SwappedValues = new int[3]
            };
            _randomSwapTriples = GetRandomizedTriples(_base.Length);
            Reset();
        }
        
        public void Dispose()
        {
            GC.Collect();
        }

        public bool MoveNext()
        {
            if(_currentTriple == _randomSwapTriples.Length)
                return false;

            //Console.WriteLine("{0} {1} {2} {3}", _firstIndex, _secondIndex, _thirdIndex, _permutation);
            _current = new Neighbour
            {
                Solution = new int[_base.Length],
                SwappedIndices = new int[3],
                SwappedValues = new int[3]
            };

            Array.Copy(_base, _current.Solution, _base.Length);

            _current.SwappedIndices[0] = _randomSwapTriples[_currentTriple].First;
            _current.SwappedIndices[1] = _randomSwapTriples[_currentTriple].Second;
            _current.SwappedIndices[2] = _randomSwapTriples[_currentTriple].Third;

            _current.SwappedValues[0] = _current.Solution[_randomSwapTriples[_currentTriple].First];
            _current.SwappedValues[1] = _current.Solution[_randomSwapTriples[_currentTriple].Second];
            _current.SwappedValues[2] = _current.Solution[_randomSwapTriples[_currentTriple].Third];

            Swap(_randomSwapTriples[_currentTriple].First,
                _randomSwapTriples[_currentTriple].Second,
                _randomSwapTriples[_currentTriple].Third,
                _randomSwapTriples[_currentTriple].Permutation);

            _currentTriple++;
            return true;
        }

        public void Reset()
        {
            Array.Copy(_base, _current.Solution, _base.Length);
            _currentTriple = 0;
        }

        public Neighbour Current
        {
            get { return _current; }
        }

        object IEnumerator.Current
        {
            get { return Current; }
        }

        private void Swap(int first, int second, int third, int permutation)
        {
            int temp;

            switch (permutation)
            {
                case 0:
                    temp = _current.Solution[second];
                    _current.Solution[second] = _current.Solution[third];
                    _current.Solution[third] = temp;
                    break;
                case 1:
                    temp = _current.Solution[first];
                    _current.Solution[first] = _current.Solution[second];
                    _current.Solution[second] = temp;
                    break;
                case 2:
                    temp = _current.Solution[first];
                    _current.Solution[first] = _current.Solution[second];
                    _current.Solution[second] = _current.Solution[third];
                    _current.Solution[third] = temp;
                    break;
                case 3:
                    temp = _current.Solution[first];
                    _current.Solution[first] = _current.Solution[third];
                    _current.Solution[third] = temp;
                    break;
                case 4:
                    temp = _current.Solution[first];
                    _current.Solution[first] = _current.Solution[third];
                    _current.Solution[third] = _current.Solution[second];
                    _current.Solution[second] = temp;
                    break;
                default:
                    throw new InvalidOperationException("Wrong permutation number, fool");
            }
        }

        private static SwapIndexTriple[] GenerateTriples(int instanceSize)
        {
            //number of triples chosen from instance times possible permutation of values inside a triple (3! - 1) == 5
            int numOfTriples = Choose(instanceSize, 3)*5;
            int counter = 0;
            var triples = new SwapIndexTriple[numOfTriples];
            for (int i = 0; i < instanceSize-2; i++)
            {
                for (int j = i+1; j < instanceSize-1; j++)
                {
                    for (int k = j+1 ; k < instanceSize; k++)
                    {
                        for (int l = 0; l < 5; l++)
                        {
                            triples[counter] = new SwapIndexTriple{First = i, Second = j, Third = k, Permutation = l};
                            counter++;
                        }
                    }
                }
            }
            return triples;
        }

        private SwapIndexTriple[] GetRandomizedTriples(int instanceSize)
        {
            var triples = GenerateTriples(instanceSize);
            for (int i = triples.Length-1; i >= 0; i--)
            {
                int toSwapWith = _random.Next(0, i);
                SwapTriples(triples, i, toSwapWith);
            }
            return triples;
        }

        private static void SwapTriples(SwapIndexTriple[] triples, int firstIndex, int secondIndex)
        {
            var temp = triples[firstIndex];
            triples[firstIndex] = triples[secondIndex];
            triples[secondIndex] = temp;
        }

        struct SwapIndexTriple
        {
            public int First { get; set; }
            public int Second { get; set; }
            public int Third { get; set; }
            public int Permutation { get; set; }
        }

        public static int Choose(int from, int count)
        {
            int result = 1;
            for(int i = 1; i <= count; i++)
            {
                result = result*(from - i + 1)/i;
            }
            return result;
        }
    }
}