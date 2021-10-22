using System.Linq;
using QueryFramework.Abstractions;
using QueryFramework.Abstractions.Builders;
using QueryFramework.Abstractions.Queries.Builders;
using QueryFramework.Core.Builders;
using QueryFramework.Core.Extensions;

namespace QueryFramework.Core.Queries.Builders.Extensions
{
    public static class SingleEntityQueryBuilderExtensions
    {
        /// <summary>
        /// Adds the condition.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="additionalConditions">The additional conditions.</param>
        /// <returns>
        /// The SingleEntityQueryBuilder instance.
        /// </returns>
        public static T Where<T>(this T instance, params IQueryConditionBuilder[] additionalConditions)
            where T : ISingleEntityQueryBuilder
        {
            instance.Conditions.AddRange(additionalConditions);
            return instance;
        }

        /// <summary>
        /// Adds the condition.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="additionalConditions">The additional conditions.</param>
        /// <returns>
        /// The SingleEntityQueryBuilder instance.
        /// </returns>
        public static T Where<T>(this T instance, params IQueryCondition[] additionalConditions)
            where T : ISingleEntityQueryBuilder
            => instance.Where(additionalConditions.Select(x => x.GetBuilder()).ToArray());

        public static T Or<T>(this T instance, params IQueryCondition[] additionalConditions)
            where T : ISingleEntityQueryBuilder
        {
            instance.Where(additionalConditions.Select(a => a.With(combination: QueryCombination.Or)).ToArray());
            return instance;
        }

        public static T Or<T>(this T instance, params IQueryConditionBuilder[] additionalConditions)
            where T : ISingleEntityQueryBuilder
        {
            instance.Where(additionalConditions.Select(a => a.WithCombination(combination: QueryCombination.Or)).ToArray());
            return instance;
        }

        public static T And<T>(this T instance, params IQueryCondition[] additionalConditions)
            where T : ISingleEntityQueryBuilder
        {
            instance.Where(additionalConditions.Select(a => a.With(combination: QueryCombination.And)).ToArray());
            return instance;
        }

        public static T And<T>(this T instance, params IQueryConditionBuilder[] additionalConditions)
            where T : ISingleEntityQueryBuilder
        {
            instance.Where(additionalConditions.Select(a => a.WithCombination(combination: QueryCombination.And)).ToArray());
            return instance;
        }

        public static T AndAny<T>(this T instance, params IQueryCondition[] additionalConditions)
            where T : ISingleEntityQueryBuilder
        {
            instance.Where(additionalConditions.Select((a, index) => a.With(openBracket: index == 0, closeBracket: index + 1 == additionalConditions.Length, combination: index == 0 ? QueryCombination.And : QueryCombination.Or)).ToArray());
            return instance;
        }

        public static T AndAny<T>(this T instance, params IQueryConditionBuilder[] additionalConditions)
            where T : ISingleEntityQueryBuilder
        {
            instance.Where(additionalConditions.Select((a, index) => a.WithOpenBracket(index == 0)
                                                                      .WithCloseBracket(index + 1 == additionalConditions.Length)
                                                                      .WithCombination(index == 0 ? QueryCombination.And : QueryCombination.Or)).ToArray());
            return instance;
        }

        public static T OrAll<T>(this T instance, params IQueryCondition[] additionalConditions)
            where T : ISingleEntityQueryBuilder
        {
            instance.Where(additionalConditions.Select((a, index) => a.With(openBracket: index == 0, closeBracket: index + 1 == additionalConditions.Length, combination: index == 0 ? QueryCombination.Or : QueryCombination.And)).ToArray());
            return instance;
        }

        public static T OrAll<T>(this T instance, params IQueryConditionBuilder[] additionalConditions)
            where T : ISingleEntityQueryBuilder
        {
            instance.Where(additionalConditions.Select((a, index) => a.WithOpenBracket(index == 0)
                                                                      .WithCloseBracket(index + 1 == additionalConditions.Length)
                                                                      .WithCombination(index == 0 ? QueryCombination.Or : QueryCombination.And)).ToArray());
            return instance;
        }

        /// <summary>
        /// Adds the order by field.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="additionalOrderByFields">The additional sort orders.</param>
        /// <returns>
        /// The SingleEntityQueryBuilder instance.
        /// </returns>
        public static T OrderBy<T>(this T instance, params IQuerySortOrderBuilder[] additionalOrderByFields)
            where T : ISingleEntityQueryBuilder
        {
            instance.OrderByFields.AddRange(additionalOrderByFields);
            return instance;
        }

        /// <summary>
        /// Adds the order by field.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="additionalOrderByFields">The additional sort orders.</param>
        /// <returns>
        /// The SingleEntityQueryBuilder instance.
        /// </returns>
        public static T OrderBy<T>(this T instance, params IQuerySortOrder[] additionalOrderByFields)
            where T : ISingleEntityQueryBuilder
            => instance.OrderBy(additionalOrderByFields.Select(x => new QuerySortOrderBuilder(x)).ToArray());

        public static T OrderBy<T>(this T instance, params string[] additionalSortOrders)
            where T : ISingleEntityQueryBuilder
            => instance.OrderBy(additionalSortOrders.Select(s => new QuerySortOrder(s)).ToArray());

        public static T OrderBy<T>(this T instance, params IQueryExpression[] additionalSortOrders)
            where T : ISingleEntityQueryBuilder
            => instance.OrderBy(additionalSortOrders.Select(s => new QuerySortOrder(s)).ToArray());

        public static T OrderBy<T>(this T instance, params IQueryExpressionBuilder[] additionalSortOrders)
            where T : ISingleEntityQueryBuilder
                => instance.OrderBy(additionalSortOrders.Select(s => new QuerySortOrder(s.Build())).ToArray());

        public static T OrderByDescending<T>(this T instance, params IQuerySortOrder[] additionalSortOrders)
            where T : ISingleEntityQueryBuilder
            => instance.OrderBy(additionalSortOrders.Select(so => new QuerySortOrder(so.Field, QuerySortOrderDirection.Descending)).ToArray());

        public static T OrderByDescending<T>(this T instance, params IQuerySortOrderBuilder[] additionalSortOrders)
            where T : ISingleEntityQueryBuilder
            => instance.OrderBy(additionalSortOrders.Select(so => new QuerySortOrder(so.Field.Build(), QuerySortOrderDirection.Descending)).ToArray());

        public static T OrderByDescending<T>(this T instance, params string[] additionalSortOrders)
            where T : ISingleEntityQueryBuilder
            => instance.OrderBy(additionalSortOrders.Select(s => new QuerySortOrder(s, QuerySortOrderDirection.Descending)).ToArray());

        public static T OrderByDescending<T>(this T instance, params IQueryExpression[] additionalSortOrders)
            where T : ISingleEntityQueryBuilder
            => instance.OrderBy(additionalSortOrders.Select(s => new QuerySortOrder(s, QuerySortOrderDirection.Descending)).ToArray());

        public static T OrderByDescending<T>(this T instance, params IQueryExpressionBuilder[] additionalSortOrders)
            where T : ISingleEntityQueryBuilder
            => instance.OrderBy(additionalSortOrders.Select(s => new QuerySortOrder(s.Build(), QuerySortOrderDirection.Descending)).ToArray());

        public static T ThenBy<T>(this T instance, params IQuerySortOrder[] additionalSortOrders)
            where T : ISingleEntityQueryBuilder
            => instance.OrderBy(additionalSortOrders);

        public static T ThenBy<T>(this T instance, params IQuerySortOrderBuilder[] additionalSortOrders)
            where T : ISingleEntityQueryBuilder
            => instance.OrderBy(additionalSortOrders);

        public static T ThenBy<T>(this T instance, params string[] additionalSortOrders)
            where T : ISingleEntityQueryBuilder
            => instance.OrderBy(additionalSortOrders);

        public static T ThenBy<T>(this T instance, params IQueryExpression[] additionalSortOrders)
            where T : ISingleEntityQueryBuilder
            => instance.OrderBy(additionalSortOrders);

        public static T ThenBy<T>(this T instance, params IQueryExpressionBuilder[] additionalSortOrders)
            where T : ISingleEntityQueryBuilder
            => instance.OrderBy(additionalSortOrders);

        public static T ThenByDescending<T>(this T instance, params IQuerySortOrder[] additionalSortOrders)
            where T : ISingleEntityQueryBuilder
            => instance.OrderByDescending(additionalSortOrders);

        public static T ThenByDescending<T>(this T instance, params IQuerySortOrderBuilder[] additionalSortOrders)
            where T : ISingleEntityQueryBuilder
            => instance.OrderByDescending(additionalSortOrders);

        public static T ThenByDescending<T>(this T instance, params string[] additionalSortOrders)
            where T : ISingleEntityQueryBuilder
            => instance.OrderByDescending(additionalSortOrders);

        public static T ThenByDescending<T>(this T instance, params IQueryExpression[] additionalSortOrders)
            where T : ISingleEntityQueryBuilder
            => instance.OrderByDescending(additionalSortOrders);

        public static T ThenByDescending<T>(this T instance, params IQueryExpressionBuilder[] additionalSortOrders)
            where T : ISingleEntityQueryBuilder
            => instance.OrderByDescending(additionalSortOrders);

        public static T Offset<T>(this T instance, int? offset)
            where T : ISingleEntityQueryBuilder
        {
            instance.Offset = offset;
            return instance;
        }

        public static T Limit<T>(this T instance, int? limit)
            where T : ISingleEntityQueryBuilder
        {
            instance.Limit = limit;
            return instance;
        }

        public static T Skip<T>(this T instance, int? offset)
            where T : ISingleEntityQueryBuilder
            => instance.Offset(offset);

        public static T Take<T>(this T instance, int? limit)
            where T : ISingleEntityQueryBuilder
            => instance.Limit(limit);
    }
}
