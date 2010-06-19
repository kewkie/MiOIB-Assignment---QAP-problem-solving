using System.Collections;
using System.Collections.Generic;

namespace Metaheuristics
{
    struct Neighbourhood : IEnumerable<Neighbour>
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
        
        public IEnumerator<Neighbour> GetEnumerator()
        {
            switch(_type)
            {
                case NeighbourhoodType.TwoSwap:
                    return new TwoSwapEnumerator(Base);
                case NeighbourhoodType.ThreeSwap:
                    return new ThreeSwapEnumerator(Base);
                case NeighbourhoodType.TwoSwapRandom:
                    return new TwoSwapRandomEnumerator(Base);
                case NeighbourhoodType.ThreeSwapRandom:
                    return new ThreeSwapRandomEnumerator(Base);
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