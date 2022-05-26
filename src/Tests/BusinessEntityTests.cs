using Orun.Extensions;
using Shouldly;
using Tests.BuildingBlocks.Fakes;
using Xunit;

namespace Tests.BuildingBlocks
{
    public class BusinessEntityTests
    {
        [Fact]
        public void BusinessEntityIsKeyEqualTo()
        {
            var entity = new FakeEntity();
            var entity2 = new FakeEntityStringKey();
            
            entity.IsKeyEqualTo(1).ShouldBeTrue();
            entity.IsKeyEqualTo(2).ShouldBeFalse();
            entity2.IsKeyEqualTo("ID").ShouldBeTrue();
            entity2.IsKeyEqualTo("WRONG").ShouldBeFalse();
        }

        [Fact]
        public void BusinessEntityGetKeyValue()
        {
            var entity = new FakeEntity();
            var entity2 = new FakeEntityStringKey();
            
            entity.GetKeyValue().ShouldBe(1);
            entity.GetKeyValue().ShouldNotBe(2);
            entity2.GetKeyValue().ShouldBe("ID");
            entity2.GetKeyValue().ShouldNotBe("WRONG");
        }
    }
}