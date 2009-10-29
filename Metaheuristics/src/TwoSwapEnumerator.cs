using System;
using System.Collections;
using System.Collections.Generic;

namespace Metaheuristics
{
    class TwoSwapEnumerator : IEnumerator<int[]>
    {
        private int[] _current;
        private readonly int[] _base;
        private int _firstIndex;
        private int _secondIndex;

        public TwoSwapEnumerator(int[] @base)
        {
            _base = @base;
            Reset();
        }

        public void Dispose()
        {
            GC.Collect();
        }

        public bool MoveNext()
        {
            Swap(_firstIndex, _secondIndex);
            
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
            Swap(_firstIndex, _secondIndex);
            return true;
        }

        private void Swap(int first, int second)
        {
            int swap = _current[first];
            _current[first] = _current[second];
            _current[second] = swap;
        }

        public void Reset()
        {
            _firstIndex = 0;
            _secondIndex = 0;
            _current = _base;
        }

        public int[] Current
        {
            get { return _current; }
        }

        object IEnumerator.Current
        {
            get { return Current; }
        }
    }
}