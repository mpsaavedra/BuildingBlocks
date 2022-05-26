using System.Linq;
using Orun.Extensions;
using Shouldly;
using Tests.BuildingBlocks.Fakes;
using Xunit;

namespace Tests.BuildingBlocks
{
    public class ObjectExtensionsTests
    {
        [Fact]
        public void InstanceOfTest()
        {
            var src = "Simple string";
            var entity = new FakeEntity();
            
            src.InstanceOfType(typeof(string)).ShouldBeTrue();
            entity.InstanceOfType(typeof(FakeEntity)).ShouldBeTrue();
            entity.InstanceOfType(typeof(string)).ShouldBeFalse();
        }

        [Fact]
        public void PropertiesOfType()
        {
            var entity = new FakeEntity();
            
            entity.PropertiesOfType(typeof(int)).Count().ShouldBe(1);
        }

        [Fact]
        public void PopulateWithMapDataTest()
        {
            var dbEntity = new FakeEntity { Id = 1, Name = "Fake 1", Deleted = false};
            var mappedInput = new FakeEntityCreateView { Deleted = true};

            dbEntity.PopulateWithMappedData(mappedInput).Id.ShouldBe(1);
            dbEntity.PopulateWithMappedData(mappedInput).Name.ShouldBe("Fake 1");
            dbEntity.PopulateWithMappedData(mappedInput).Deleted.ShouldBeTrue();
        }
    }
}