using CrossCutting.Data.Abstractions;
using QueryFramework.Abstractions;

namespace QueryFramework.SqlServer.Tests.Repositories
{
    public interface ITestRepository : IRepository<TestEntity, TestEntityIdentity>
    {
    }
}
