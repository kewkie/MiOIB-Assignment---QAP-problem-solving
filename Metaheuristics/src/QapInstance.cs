namespace Metaheuristics
{
    public class QapInstance
    {
        private readonly int _instanceSize;
        private readonly int[] _distances;
        private readonly int[] _costs;

        public QapInstance(int size)
        {
            _instanceSize = size;
            _distances = new int[size*size];
            _costs = new int[size*size];
            OptimalSolution = new int[size];
        }

        public int OptimalScore { get; set; }

        public int[] OptimalSolution { get; set; }

        public int[] DistanceMatrix
        {
            get
            {
                return _distances;
            }
        }

        public int[] CostMatrix
        {
            get
            {
                return _costs;
            }
        }

        public int Size
        {
            get
            {
                return _instanceSize;
            }
        }

        public string Name { get; set; }
    }
}
