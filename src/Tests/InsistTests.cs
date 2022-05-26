using System;
using System.Linq;
using Orun;
using Shouldly;
using Tests.BuildingBlocks.Fakes;
using Xunit;

namespace Tests.BuildingBlocks
{
    public class InsistTests
    {
        private readonly string  _testValue = "not null";
            
        [Fact]
        public void InsistIsNotNull()
        {
            Insist.IsNotNull(_testValue, nameof(_testValue));
        }

        [Fact]
        public void InsistIsNotNullOrWhiteSpace()
        {
            Insist.IsNotNullOrWhiteSpace(_testValue, nameof(_testValue));
        }

        [Fact]
        public void InsistTrue()
        {
            Insist.True<Exception>(1 == 1, () => "Is not true?");
        }

        [Fact]
        public void InsistFalse()
        {
            Insist.False<Exception>(1 == 2, () => "Is not false?");
        }

        [Fact]
        public void InsistThrowAndCachedException()
        {
            try
            {
                Insist.Throw<FakeInsistException>(nameof(FakeInsistException));
            }
            catch
            {
                Insist.CachedExceptionTypes.Count().ShouldBe(1);
            }

        }
    }
}