using QueryFramework.Abstractions;

namespace QueryFramework.SqlServer.Tests.Repositories
{
    public interface ITestRepository : IQueryProcessor<ITestQuery, TestEntity>
    {
        TestEntity Add(TestEntity instance);

        TestEntity Update(TestEntity instance);

        TestEntity Delete(TestEntity instance);

        TestEntity? Find(TestEntityIdentity identity);
    }
}
