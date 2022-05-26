using System;

namespace Tests.BuildingBlocks.Fakes
{
    [Serializable]
    public class FakeUser
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public string? Email { get; set; }

        public int Age { get; set; }

        public string[]? Permissions { get; set; }
    }
}