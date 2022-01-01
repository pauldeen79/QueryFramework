using System.Linq;
using QueryFramework.Abstractions;
using QueryFramework.Abstractions.Builders;
using QueryFramework.Abstractions.Extensions.Builders;
using QueryFramework.Abstractions.Queries.Builders;
using QueryFramework.Core.Builders;

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

        public static T Or<T>(this T instance, params IQueryConditionBuilder[] additionalConditions)
            where T : ISingleEntityQueryBuilder
        {
            instance.Where(additionalConditions.Select(a => a.WithCombination(combination: QueryCombination.Or)).ToArray());
            return instance;
        }

        public static T And<T>(this T instance, params IQueryConditionBuilder[] additionalConditions)
            where T : ISingleEntityQueryBuilder
        {
            instance.Where(additionalConditions.Select(a => a.WithCombination(combination: QueryCombination.And)).ToArray());
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

        public static T OrderBy<T>(this T instance, params string[] additionalSortOrders)
            where T : ISingleEntityQueryBuilder
            => instance.OrderBy(additionalSortOrders.Select(s => new QuerySortOrderBuilder(s)).ToArray());

        public static T OrderBy<T>(this T instance, params IQueryExpressionBuilder[] additionalSortOrders)
            where T : ISingleEntityQueryBuilder
                => instance.OrderBy(additionalSortOrders.Select(s => new QuerySortOrderBuilder(s.Build())).ToArray());

        public static T OrderByDescending<T>(this T instance, params IQuerySortOrderBuilder[] additionalSortOrders)
            where T : ISingleEntityQueryBuilder
            => instance.OrderBy(additionalSortOrders.Select(so => new QuerySortOrderBuilder(so.Field.Build(), QuerySortOrderDirection.Descending)).ToArray());

        public static T OrderByDescending<T>(this T instance, params string[] additionalSortOrders)
            where T : ISingleEntityQueryBuilder
            => instance.OrderBy(additionalSortOrders.Select(s => new QuerySortOrderBuilder(s, QuerySortOrderDirection.Descending)).ToArray());

        public static T OrderByDescending<T>(this T instance, params IQueryExpressionBuilder[] additionalSortOrders)
            where T : ISingleEntityQueryBuilder
            => instance.OrderBy(additionalSortOrders.Select(s => new QuerySortOrderBuilder(s.Build(), QuerySortOrderDirection.Descending)).ToArray());

        public static T ThenBy<T>(this T instance, params IQuerySortOrderBuilder[] additionalSortOrders)
            where T : ISingleEntityQueryBuilder
            => instance.OrderBy(additionalSortOrders);

        public static T ThenBy<T>(this T instance, params string[] additionalSortOrders)
            where T : ISingleEntityQueryBuilder
            => instance.OrderBy(additionalSortOrders);

        public static T ThenBy<T>(this T instance, params IQueryExpressionBuilder[] additionalSortOrders)
            where T : ISingleEntityQueryBuilder
            => instance.OrderBy(additionalSortOrders);

        public static T ThenByDescending<T>(this T instance, params IQuerySortOrderBuilder[] additionalSortOrders)
            where T : ISingleEntityQueryBuilder
            => instance.OrderByDescending(additionalSortOrders);

        public static T ThenByDescending<T>(this T instance, params string[] additionalSortOrders)
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
