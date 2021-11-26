using System.Diagnostics.CodeAnalysis;
using System.Text;
using QueryFramework.SqlServer.Abstractions;

namespace QueryFramework.SqlServer.Tests.Repositories
{
    [ExcludeFromCodeCoverage]
    public class TestEntityQueryProcessorSettings : IQueryProcessorSettings
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
        public bool ValidateFieldNames => true;
        public int InitialParameterNumber => 0;
    }
}
