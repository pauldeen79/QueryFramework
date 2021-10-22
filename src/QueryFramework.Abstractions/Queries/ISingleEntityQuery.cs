using System.Collections.Generic;

namespace QueryFramework.Abstractions.Queries
{
    /// <summary>
    /// Interface for query on a single entity, with basic capabilities. (filter conditions, item count limitation, sort order)
    /// </summary>
    public interface ISingleEntityQuery
    {
        /// <summary>
        /// Maximum number of records to get, or null for no limit.
        /// </summary>
        int? Limit { get; }

        /// <summary>
        /// Gets the offset.
        /// </summary>
        /// <value>
        /// The offset.
        /// </value>
        int? Offset { get; }

        /// <summary>
        /// Gets the conditions.
        /// </summary>
        /// <value>
        /// The conditions.
        /// </value>
        IReadOnlyCollection<IQueryCondition> Conditions { get; }

        /// <summary>
        /// Gets the order by fields.
        /// </summary>
        /// <value>
        /// The order by fields.
        /// </value>
        IReadOnlyCollection<IQuerySortOrder> OrderByFields { get; }
    }
}
