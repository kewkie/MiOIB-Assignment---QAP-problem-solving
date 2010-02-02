using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Metaheuristics
{
    class ThreeSwapEnumerator : IEnumerator<int[]>
    {
        private readonly int[] _current;
        private readonly int[] _base;
        private int _firstIndex;
        private int _secondIndex;
        private int _thirdIndex;
        private int _permutation;
        
        public ThreeSwapEnumerator(int[] @base)
        {
            _base = @base;
            _current = new int[_base.Length];
            Reset();
        }
        
        public void Dispose()
        {
            GC.Collect();
        }

        public bool MoveNext()
        {
            //Console.WriteLine("{0} {1} {2} {3}", _firstIndex, _secondIndex, _thirdIndex, _permutation);
            _permutation++;
            if (_permutation % 5 == 0)
            {
                _thirdIndex++;
                if (_thirdIndex == _base.Length)
                {
                    _secondIndex++;
                    if (_secondIndex == _base.Length - 1)
                    {
                        _firstIndex++;
                        if (_firstIndex == _base.Length - 2)
                        {
                            return false;
                        }
                        _secondIndex = _firstIndex + 1;
                    }
                    _thirdIndex = _secondIndex + 1;
                }
                _permutation = 0;
            }
            Swap(_firstIndex, _secondIndex, _thirdIndex, _permutation);
            return true;
        }

        private void Swap(int first, int second, int third, int permutation)
        {
            Array.Copy(_base, _current, _base.Length);
            int temp;

            switch(permutation)
            {
                case 0:
                    temp = _current[second];
                    _current[second] = _current[third];
                    _current[third] = temp;
                    break;
                case 1:
                    temp = _current[first];
                    _current[first] = _current[second];
                    _current[second] = temp;
                    break;
                case 2:
                    temp = _current[first];
                    _current[first] = _current[second];
                    _current[second] = _current[third];
                    _current[third] = temp;
                    break;
                case 3:
                    temp = _current[first];
                    _current[first] = _current[third];
                    _current[third] = temp;
                    break;
                case 4:
                    temp = _current[first];
                    _current[first] = _current[third];
                    _current[third] = _current[second];
                    _current[second] = temp;
                    break;
                default:
                    throw new InvalidOperationException("Wrong permutation number, fool");
            }
        }

        public void Reset()
        {
            _firstIndex = 0;
            _secondIndex = 1;
            _thirdIndex = 2;
            _permutation = 0;
            Array.Copy(_base, _current, _base.Length);
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