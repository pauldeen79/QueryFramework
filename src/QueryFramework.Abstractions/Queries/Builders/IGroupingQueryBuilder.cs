using System.Collections.Generic;
using QueryFramework.Abstractions.Builders;

namespace QueryFramework.Abstractions.Queries.Builders
{
    public interface IGroupingQueryBuilder : ISingleEntityQueryBuilder
    {
        List<IQueryExpressionBuilder> GroupByFields { get; set; }
        List<IQueryConditionBuilder> HavingFields { get; set; }
    }
}
