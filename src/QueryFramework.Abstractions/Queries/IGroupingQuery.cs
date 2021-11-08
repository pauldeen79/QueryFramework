using CrossCutting.Common;

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
        ValueCollection<IQueryExpression> GroupByFields { get; }

        /// <summary>
        /// Gets the having fields.
        /// </summary>
        /// <value>
        /// The having fields.
        /// </value>
        ValueCollection<IQueryCondition> HavingFields { get; }
    }
}
