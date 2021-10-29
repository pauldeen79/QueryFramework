using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using QueryFramework.Abstractions;
using QueryFramework.Abstractions.Queries;

namespace QueryFramework.SqlServer.Tests.TestHelpers
{
    [ExcludeFromCodeCoverage]
    internal class DynamicQueryMock : IDynamicQuery
    {
        public int? Limit { get; set; }
        public int? Offset { get; set; }
        public IReadOnlyCollection<IQueryCondition> Conditions { get; set; }
        public IReadOnlyCollection<IQuerySortOrder> OrderByFields { get; set; }

        public ISingleEntityQuery Process() => new DynamicQueryMock { Limit = 10, Offset = 10 };

        public DynamicQueryMock()
        {
            Conditions = new List<IQueryCondition>();
            OrderByFields = new List<IQuerySortOrder>();
        }
    }
}
