using QueryFramework.Abstractions.Builders;
using System.Collections.Generic;

namespace QueryFramework.Abstractions.Queries.Builders
{
    /// <summary>
    /// Interface for a single entity query builder.
    /// </summary>
    public interface ISingleEntityQueryBuilder
    {
        /// <summary>Gets or sets the limit.</summary>
        /// <value>The limit.</value>
        int? Limit { get; set; }

        /// <summary>Gets or sets the offset.</summary>
        /// <value>The offset.</value>
        int? Offset { get; set; }

        /// <summary>Gets the conditions.</summary>
        /// <value>The conditions.</value>
        List<IQueryConditionBuilder> Conditions { get; set; }

        /// <summary>Gets the order by fields.</summary>
        /// <value>The order by fields.</value>
        List<IQuerySortOrderBuilder> OrderByFields { get; set; }
    }
}
