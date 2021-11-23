using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using QueryFramework.Abstractions;
using QueryFramework.Core.Queries;

namespace QueryFramework.SqlServer.Tests.Repositories
{
    [ExcludeFromCodeCoverage]
    public record TestQuery : SingleEntityQuery, ITestQuery
    {
        public TestQuery()
        {
        }

        public TestQuery(int? limit, int? offset, IEnumerable<IQueryCondition> conditions, IEnumerable<IQuerySortOrder> orderByFields) : base(limit, offset, conditions, orderByFields)
        {
        }
    }
}
