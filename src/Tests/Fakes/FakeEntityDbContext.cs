using Microsoft.EntityFrameworkCore;

namespace Tests.BuildingBlocks.Fakes
{
    public class FakeEntityDbContext : DbContext
    {
        public FakeEntityDbContext(DbContextOptions options) : base(options)
        {
        }
        
        public virtual DbSet<FakeEntity> FakeEntities { get; set; }
    }
}