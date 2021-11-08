using System.Collections.Generic;
using System.Linq;
using CrossCutting.Common;
using QueryFramework.Abstractions;
using QueryFramework.Abstractions.Queries;

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
            Conditions = new ValueCollection<IQueryCondition>(conditions ?? Enumerable.Empty<IQueryCondition>());
            OrderByFields = new ValueCollection<IQuerySortOrder>(orderByFields ?? Enumerable.Empty<IQuerySortOrder>());
        }

        public int? Limit { get; }
        public int? Offset { get; }
        public ValueCollection<IQueryCondition> Conditions { get; }
        public ValueCollection<IQuerySortOrder> OrderByFields { get; }
    }
}
