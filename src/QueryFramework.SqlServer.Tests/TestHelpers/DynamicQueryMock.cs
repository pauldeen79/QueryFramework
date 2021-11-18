using System.Diagnostics.CodeAnalysis;
using CrossCutting.Common;
using QueryFramework.Abstractions;
using QueryFramework.Abstractions.Queries;

namespace QueryFramework.SqlServer.Tests.TestHelpers
{
    [ExcludeFromCodeCoverage]
    internal class DynamicQueryMock : IDynamicQuery<ISingleEntityQuery>
    {
        public int? Limit { get; set; }
        public int? Offset { get; set; }
        public ValueCollection<IQueryCondition> Conditions { get; set; }
        public ValueCollection<IQuerySortOrder> OrderByFields { get; set; }

        public ISingleEntityQuery Process() => new DynamicQueryMock { Limit = 10, Offset = 10 };

        public DynamicQueryMock()
        {
            Conditions = new ValueCollection<IQueryCondition>();
            OrderByFields = new ValueCollection<IQuerySortOrder>();
        }
    }
}
