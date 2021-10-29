using System.Diagnostics.CodeAnalysis;

namespace QueryFramework.SqlServer.Tests.Repositories
{
    [ExcludeFromCodeCoverage]
    public class TestEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
