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
                case NeighbourhoodType.Two_Swap:
                    return new TwoSwapEnumerator(Base);
                case NeighbourhoodType.Three_Swap:
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