using System.Collections.Generic;

namespace QueryFramework.Abstractions.Queries
{
    public interface IGroupingQuery : ISingleEntityQuery
    {
        /// <summary>
        /// Gets the group by fields.
        /// </summary>
        /// <value>
        /// The group by fields.
        /// </value>
        IReadOnlyCollection<IQueryExpression> GroupByFields { get; }

        /// <summary>
        /// Gets the having fields.
        /// </summary>
        /// <value>
        /// The having fields.
        /// </value>
        IReadOnlyCollection<IQueryCondition> HavingFields { get; }
    }
}
