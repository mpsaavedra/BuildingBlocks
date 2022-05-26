using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Orun.BuildingBlocks.Domain;

namespace Tests.BuildingBlocks.Fakes
{
    public interface IFakeEntityRepository: IRepository<int, FakeEntity>
    {
    }

    public class FakeEntityRepository : Repository<int, FakeEntity>,
        IFakeEntityRepository
    {
        public FakeEntityRepository(IMapper mapper, DbContext context) : base(mapper, context)
        {
        }
    }
}
