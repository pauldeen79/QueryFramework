using System.Collections.Generic;
using QueryFramework.Abstractions.Builders;

namespace QueryFramework.Abstractions.Queries.Builders
{
    public interface ISingleEntityQueryBuilderBase
    {
        int? Limit { get; set; }
        int? Offset { get; set; }
        List<IQueryConditionBuilder> Conditions { get; set; }
        List<IQuerySortOrderBuilder> OrderByFields { get; set; }
    }
}
