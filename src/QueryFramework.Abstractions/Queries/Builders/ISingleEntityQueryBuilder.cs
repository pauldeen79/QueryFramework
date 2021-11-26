using QueryFramework.Abstractions.Builders;
using System.Collections.Generic;

namespace QueryFramework.Abstractions.Queries.Builders
{
    public interface ISingleEntityQueryBuilder
    {
        int? Limit { get; set; }
        int? Offset { get; set; }
        List<IQueryConditionBuilder> Conditions { get; set; }
        List<IQuerySortOrderBuilder> OrderByFields { get; set; }
    }
}
