using System.Linq;
using QueryFramework.Abstractions.Builders;
using QueryFramework.Abstractions.Queries.Builders;
using QueryFramework.Core.Builders;

namespace QueryFramework.Core.Extensions
{
    public static class GroupingQueryBuilderExtensions
    {
        /// <summary>
        /// Adds group by fields.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="additionalFieldNames">Group by fields to add.</param>
        /// <returns>
        /// The FieldSelectionQueryBuilder instance.
        /// </returns>
        public static T GroupBy<T>(this T instance, params IQueryExpressionBuilder[] additionalFieldNames)
            where T : IGroupingQueryBuilder
        {
            instance.GroupByFields.AddRange(additionalFieldNames);
            return instance;
        }

        /// <summary>
        /// Adds group by fields.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="additionalFieldNames">Group by fields to add.</param>
        /// <returns>
        /// The FieldSelectionQueryBuilder instance.
        /// </returns>
        public static T GroupBy<T>(this T instance, params string[] additionalFieldNames)
            where T : IGroupingQueryBuilder
            => instance.GroupBy(additionalFieldNames.Select(s => new QueryExpressionBuilder(s)).ToArray());

        /// <summary>
        /// Adds having fields.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="additionalFieldNames">Having fields to add.</param>
        /// <returns>
        /// The FieldSelectionQueryBuilder instance.
        /// </returns>
        public static T Having<T>(this T instance, params IQueryConditionBuilder[] additionalFieldNames)
            where T : IGroupingQueryBuilder
        {
            instance.HavingFields.AddRange(additionalFieldNames);
            return instance;
        }
    }
}
