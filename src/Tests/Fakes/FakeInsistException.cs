using System;

namespace Tests.BuildingBlocks.Fakes
{
    public class FakeInsistException : Exception
    {
        public FakeInsistException()
        {
        }

        public FakeInsistException(string? message) : base(message)
        {
        }
    }
}