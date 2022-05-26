using Shouldly;
using Tests.BuildingBlocks.Fakes;
using Xunit;

namespace Tests.BuildingBlocks
{
    public class TypeTests
    {
        [Fact]
        public void TypeOfInstance()
        {
            const string src = "This is an string value";
            var isOfType = FakeTypes.ReferenceTypes.ClassTypes.String_Empty;
            
            var areTheSame = src.GetType() == isOfType.GetType();
            
            areTheSame.ShouldBe(true);
        }
    }
}