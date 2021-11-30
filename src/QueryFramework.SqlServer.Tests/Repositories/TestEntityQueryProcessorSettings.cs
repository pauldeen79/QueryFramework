using System.Diagnostics.CodeAnalysis;
using System.Text;
using CrossCutting.Data.Abstractions;
using QueryFramework.SqlServer.Abstractions;

namespace QueryFramework.SqlServer.Tests.Repositories
{
    [ExcludeFromCodeCoverage]
    public class TestEntityQueryProcessorSettings : IPagedDatabaseEntityRetrieverSettings
    {
        public string TableName => "TestEntity";
        public string Fields => "Field1, Field2, Field3";
        public string DefaultOrderBy
        {
            get
            {
                var builder = new StringBuilder();
                builder.Append("[Name]");
                return builder.ToString();
            }
        }
        public string DefaultWhere
        {
            get
            {
                var builder = new StringBuilder();

                return builder.ToString();
            }
        }
        public int? OverridePageSize => null;
    }
}
