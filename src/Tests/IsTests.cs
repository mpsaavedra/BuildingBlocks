using System;
using System.Linq;
using Orun;
using Shouldly;
using Tests.BuildingBlocks.Fakes;
using Xunit;

namespace Tests.BuildingBlocks
{
    public class IsTests
    {
        [Fact]
        public void IsNullOrEmpty()
        {
            var isNull = Is.NullOrEmpty(FakeTypes.ValueTypes.SimpleTypes.Bool_Null);
            var isNullAny = Is.NullOrEmpty(
                FakeTypes.ValueTypes.SimpleTypes.Char_Null,
                "this has value");
            
            var notNull = Is.NullOrEmpty("this has value");
            var notNullAny = Is.NullOrEmpty("this has value", "another value");
            
            isNull.ShouldBe(true);
            isNullAny.ShouldBe(true);
            notNull.ShouldBe(false);
            notNullAny.ShouldBe(false);
        }

        [Fact]
        public void IsSourceNullOrEmpty()
        {
            const string val = "this has a value";
            var src = val;
            
            var dst = src.IsNullOrEmpty("Source could not be null or empty");
            
            src.ShouldBe(val);
            dst.ShouldBe(val);
        }

        public void IsSourceNullOrEmptyThrow()
        {
            try
            {
                const string mark = "this is mark message";
                const string msg = "Source is null";
                string? src = null;
                
                var dst1 = msg.IsNullOrEmpty(mark);
                var dst2 = src.IsNullOrEmpty(msg);
                
            }
            catch (Exception ex)
            {
                const string msg = "Source is null";
                ex.Message.ShouldBe(msg);
            }
        }

        [Fact]
        public void ThenIfSourceIsNullOrEmptyReturnOptional()
        {
            const string source = "test string";
            const string optional = "optional string";
            string nullString = null;
            
            source.ThenIfNullOrEmpty(optional).ShouldBe(source);
            nullString.ThenIfNullOrEmpty(optional).ShouldBe(optional);
        }
    }
}