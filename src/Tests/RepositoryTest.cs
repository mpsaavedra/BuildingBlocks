using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Orun;
using Shouldly;
using Tests.BuildingBlocks.Fakes;
using Xunit;

namespace Tests.BuildingBlocks
{
    public class RepositoryTest
    {
        private readonly FakeEntityDbContext _dbContext;
        private readonly IServiceProvider servicesProvider;

        public RepositoryTest()
        {
            var options = new DbContextOptionsBuilder<FakeEntityDbContext>()
                .UseInMemoryDatabase(databaseName: "EmployeeDataBase")
                .Options;

            _dbContext = new FakeEntityDbContext(options);

            _dbContext.FakeEntities.Add(new FakeEntity { Id = 1, Name = "Fake 1"});
            _dbContext.FakeEntities.Add(new FakeEntity { Id = 2, Name = "Fake 2"});
            _dbContext.FakeEntities.Add(new FakeEntity { Id = 3, Name = "Fake 3"});
            _dbContext.FakeEntities.Add(new FakeEntity { Id = 4, Name = "Fake 4"});
            _dbContext.FakeEntities.Add(new FakeEntity { Id = 5, Name = "Fake 5"});
            _dbContext.SaveChanges();

            servicesProvider = new ServiceCollection()
                .BuildServiceProvider();
        }

        [Fact]
        public void Testing()
        {
            _dbContext.FakeEntities.Count().ShouldBe(5);
        }
    }
}