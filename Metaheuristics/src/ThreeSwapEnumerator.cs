using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Metaheuristics
{
    class ThreeSwapEnumerator : IEnumerator<int>
    {
        private int[] _current;
        private readonly int[] _base;
        private int _firstIndex;
        private int _secondIndex;
        private int _thirdIndex;
        
        public ThreeSwapEnumerator(int[] @base)
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
            throw new NotImplementedException();
        }

        private void Swap(int first, int second, int third)
        {
            
        }

        public void Reset()
        {
            _firstIndex = 0;
            _secondIndex = 0;
            _thirdIndex = 0;
            _current = _base;
        }

        public int Current
        {
            get { throw new NotImplementedException(); }
        }

        object IEnumerator.Current
        {
            get { return Current; }
        }
    }
}