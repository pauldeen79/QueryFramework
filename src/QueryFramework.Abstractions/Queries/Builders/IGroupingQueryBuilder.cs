using System.Collections.Generic;
using QueryFramework.Abstractions.Builders;

namespace QueryFramework.Abstractions.Queries.Builders
{
    public interface IGroupingQueryBuilder : ISingleEntityQueryBuilderBase
    {
        List<IQueryExpressionBuilder> GroupByFields { get; set; }
        List<IQueryConditionBuilder> HavingFields { get; set; }
        IGroupingQuery Build();
    }
}
