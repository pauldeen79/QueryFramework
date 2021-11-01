using QueryFramework.Abstractions;
using QueryFramework.Abstractions.Queries;
using System.Collections.Generic;
using System.Linq;

namespace QueryFramework.Core.Queries
{
    public record SingleEntityQuery : ISingleEntityQuery
    {
        public SingleEntityQuery(IEnumerable<IQueryCondition>? conditions = null,
                                 IEnumerable<IQuerySortOrder>? orderByFields = null,
                                 int? limit = null,
                                 int? offset = null)
        {
            Limit = limit;
            Offset = offset;
            Conditions = new List<IQueryCondition>(conditions ?? Enumerable.Empty<IQueryCondition>()).AsReadOnly();
            OrderByFields = new List<IQuerySortOrder>(orderByFields ?? Enumerable.Empty<IQuerySortOrder>()).AsReadOnly();
        }

        public int? Limit { get; }
        public int? Offset { get; }
        public IReadOnlyCollection<IQueryCondition> Conditions { get; }
        public IReadOnlyCollection<IQuerySortOrder> OrderByFields { get; }
    }
}
