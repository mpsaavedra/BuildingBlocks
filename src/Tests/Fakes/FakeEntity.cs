using Orun.BuildingBlocks.Domain;
using Orun.Domain;

namespace Tests.BuildingBlocks.Fakes
{
    public class FakeEntity : BusinessEntity<int>
    {
        public FakeEntity()
        {
            Id = 1;
            Name = nameof(FakeEntity);
        }

        public int Id { get; set; }
        
        public string Name { get; set; }
    }

    public class FakeEntityStringKey : BusinessEntity<string>
    {
        public FakeEntityStringKey()
        {
            FakeEntityStringKeyId = "ID";
        }

        public string FakeEntityStringKeyId { get; set; }
    }
}