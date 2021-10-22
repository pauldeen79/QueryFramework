using System.Collections.Generic;
using QueryFramework.Abstractions.Builders;

namespace QueryFramework.Abstractions.Queries.Builders
{
    public interface IGroupingQueryBuilder : ISingleEntityQueryBuilder
    {
        /// <summary>Gets the group by fields.</summary>
        /// <value>The group by fields.</value>
        List<IQueryExpressionBuilder> GroupByFields { get; set; }

        /// <summary>Gets the having fields.</summary>
        /// <value>The having fields.</value>
        List<IQueryConditionBuilder> HavingFields { get; set; }
    }
}
