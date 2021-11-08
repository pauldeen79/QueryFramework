using System.Collections.Generic;
using QueryFramework.Abstractions;
using QueryFramework.Abstractions.Queries;

namespace QueryFramework.SqlServer.Tests.Repositories
{
    public record TestQuery : Core.Queries.SingleEntityQuery, ITestQuery
    {
        public TestQuery(IEnumerable<IQueryCondition>? conditions = null, IEnumerable<IQuerySortOrder>? orderByFields = null, int? limit = null, int? offset = null) : base(conditions, orderByFields, limit, offset)
        {
        }

        public TestQuery(ISingleEntityQuery simpleEntityQuery) : this(simpleEntityQuery.Conditions, simpleEntityQuery.OrderByFields, simpleEntityQuery.Limit, simpleEntityQuery.Offset)
        {
        }
    }
}
