using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using CrossCutting.Data.Abstractions;
using CrossCutting.Data.Sql.Extensions;

namespace QueryFramework.SqlServer.Tests.Repositories
{
    [ExcludeFromCodeCoverage]
    public class TestEntityMapper : IDataReaderMapper<TestEntity>
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
}
