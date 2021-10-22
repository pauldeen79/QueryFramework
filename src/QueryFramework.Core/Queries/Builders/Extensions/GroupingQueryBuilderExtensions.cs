using System.Linq;
using QueryFramework.Abstractions;
using QueryFramework.Abstractions.Builders;
using QueryFramework.Abstractions.Queries.Builders;
using QueryFramework.Core.Builders;
using QueryFramework.Core.Extensions;

namespace QueryFramework.Core.Queries.Builders.Extensions
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
        public static T GroupBy<T>(this T instance, params IQueryExpression[] additionalFieldNames)
            where T : IGroupingQueryBuilder
            => instance.GroupBy(additionalFieldNames.Select(x => new QueryExpressionBuilder(x)).ToArray());

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
            => instance.GroupBy(additionalFieldNames.Select(s => new QueryExpression(s)).ToArray());

        /// <summary>
        /// Adds having fields.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="additionalFieldNames">Having fields to add.</param>
        /// <returns>
        /// The FieldSelectionQueryBuilder instance.
        /// </returns>
        public static T Having<T>(this T instance, params IQueryCondition[] additionalFieldNames)
            where T : IGroupingQueryBuilder
        {
            instance.HavingFields.AddRange(additionalFieldNames.Select(x => x.GetBuilder()).ToArray());
            return instance;
        }

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
