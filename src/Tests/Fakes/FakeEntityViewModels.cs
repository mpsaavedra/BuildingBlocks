namespace Tests.BuildingBlocks.Fakes
{
    public class FakeEntityCreateView
    {
        public string Name { get; set; }
        
        public bool Deleted { get; set; }
    }

    public class FakeEntityUpdateView : FakeEntityCreateView
    {
        
    }
}