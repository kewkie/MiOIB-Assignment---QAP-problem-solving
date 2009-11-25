using System.Collections;
using System.Collections.Generic;

namespace Metaheuristics
{
    struct Neighbourhood : IEnumerable<int[]>
    {
        private readonly NeighbourhoodType _type;
        
        public int[] Base
        {
            get; set;
        }
        
        public Neighbourhood(int[] basePerm, NeighbourhoodType type) : this()
        {
            Base = basePerm;
            _type = type;

        }
        
        public IEnumerator<int[]> GetEnumerator()
        {
            switch(_type)
            {
                case NeighbourhoodType.TwoSwap:
                    return new TwoSwapEnumerator(Base);
                case NeighbourhoodType.ThreeSwap:
                    return null;
                default:
                    return new TwoSwapEnumerator(Base);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}