using System;
using System.Collections;
using System.Collections.Generic;

namespace Metaheuristics
{
    class TwoSwapEnumerator : IEnumerator<Neighbour>
    {
        private Neighbour _current;
        private readonly int[] _base;
        private int _firstIndex;
        private int _secondIndex;

        public TwoSwapEnumerator(int[] @base)
        {
            _base = @base;
            _current = new Neighbour
                           {
                               Solution = new int[_base.Length],
                               SwappedIndices = new int[2],
                               SwappedValues = new int[2]
                           };
            Reset();
        }

        public void Dispose()
        {
            GC.Collect();
        }

        public bool MoveNext()
        {
            //Swap(_firstIndex, _secondIndex);
            _current = new Neighbour
            {
                Solution = new int[_base.Length],
                SwappedIndices = new int[2],
                SwappedValues = new int[2]
            };
            Array.Copy(_base, _current.Solution, _base.Length);
            _secondIndex++;
            if(_secondIndex == _base.Length)
            {
                _firstIndex++;
                if (_firstIndex == _base.Length-1)
                {
                    return false;
                }
                _secondIndex = _firstIndex + 1;
            }
            _current.SwappedIndices[0] = _firstIndex;
            _current.SwappedIndices[1] = _secondIndex;
            _current.SwappedValues[0] = _current.Solution[_firstIndex];
            _current.SwappedValues[1] = _current.Solution[_secondIndex];
            Swap(_firstIndex, _secondIndex);
            return true;
        }

        private void Swap(int first, int second)
        {
            int swap = _current.Solution[first];
            _current.Solution[first] = _current.Solution[second];
            _current.Solution[second] = swap;
        }

        public void Reset()
        {
            _firstIndex = 0;
            _secondIndex = 0;
            Array.Copy(_base, _current.Solution, _base.Length);
        }

        public Neighbour Current
        {
            get { return _current; }
        }

        object IEnumerator.Current
        {
            get { return Current; }
        }
    }
}