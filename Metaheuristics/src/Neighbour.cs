namespace Metaheuristics
{
    struct Neighbour
    {
        public int[] Solution { get; set; }
        public int[] SwappedIndices { get; set; }
        public int[] SwappedValues { get; set; }
    }
}