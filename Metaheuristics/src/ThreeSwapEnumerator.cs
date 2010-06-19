using System;
using System.Collections;
using System.Collections.Generic;

namespace Metaheuristics
{
    class ThreeSwapEnumerator : IEnumerator<Neighbour>
    {
        private Neighbour _current;
        private readonly int[] _base;
        private int _firstIndex;
        private int _secondIndex;
        private int _thirdIndex;
        private int _permutation;
        
        public ThreeSwapEnumerator(int[] @base)
        {
            _base = @base;
            _current = new Neighbour
                           {
                               Solution = new int[_base.Length],
                               SwappedIndices = new int[3],
                               SwappedValues = new int[3]
                           };
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

            _current.SwappedIndices[0] = _firstIndex;
            _current.SwappedIndices[1] = _secondIndex;
            _current.SwappedIndices[2] = _thirdIndex;

            _current.SwappedValues[0] = _current.Solution[_firstIndex];
            _current.SwappedValues[1] = _current.Solution[_secondIndex];
            _current.SwappedValues[2] = _current.Solution[_thirdIndex];
            return true;
        }

        private void Swap(int first, int second, int third, int permutation)
        {
            Array.Copy(_base, _current.Solution, _base.Length);
            int temp;

            switch(permutation)
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

        public void Reset()
        {
            _firstIndex = 0;
            _secondIndex = 1;
            _thirdIndex = 2;
            _permutation = 0;
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