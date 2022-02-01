namespace QueryFramework.SqlServer.Tests.Repositories;

public class TestEntityMapper : IDatabaseEntityMapper<TestEntity>
{
    public TestEntity Map(IDataReader reader)
    {
        var instance = new TestEntity
        {
            Id = reader.GetInt32("Id"),
            Name = reader.GetString("Name")
        };

        return instance;
    }
}
