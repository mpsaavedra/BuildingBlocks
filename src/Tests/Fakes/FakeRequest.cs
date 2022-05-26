namespace Tests.BuildingBlocks.Fakes
{
    public class Paging
    {
        public string Property { get; set; }
        public bool Ascending { get; set; } = true;
        public int Index { get; set; } = 1;
        public int Size { get; set; } = 20;
    }

    public enum Filters
    {
        
        GreaterThenEqual,
    }
    public class FakeRequest
    {
        
    }
}