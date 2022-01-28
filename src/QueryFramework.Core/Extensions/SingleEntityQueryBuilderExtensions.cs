using System.Linq;
using QueryFramework.Abstractions;
using QueryFramework.Abstractions.Builders;
using QueryFramework.Abstractions.Extensions;
using QueryFramework.Abstractions.Queries.Builders;
using QueryFramework.Core.Builders;

namespace QueryFramework.Core.Extensions
{
    public static class SingleEntityQueryBuilderExtensions
    {
        public static T Where<T>(this T instance, params IQueryConditionBuilder[] additionalConditions)
            where T : ISingleEntityQueryBuilderBase
        {
            instance.Conditions.AddRange(additionalConditions);
            return instance;
        }

        public static T Or<T>(this T instance, params IQueryConditionBuilder[] additionalConditions)
            where T : ISingleEntityQueryBuilderBase
        {
            instance.Where(additionalConditions.Select(a => a.WithCombination(combination: QueryCombination.Or)).ToArray());
            return instance;
        }

        public static T And<T>(this T instance, params IQueryConditionBuilder[] additionalConditions)
            where T : ISingleEntityQueryBuilderBase
        {
            instance.Where(additionalConditions.Select(a => a.WithCombination(combination: QueryCombination.And)).ToArray());
            return instance;
        }

        public static T AndAny<T>(this T instance, params IQueryConditionBuilder[] additionalConditions)
            where T : ISingleEntityQueryBuilderBase
        {
            instance.Where(additionalConditions.Select((a, index) => a.WithOpenBracket(index == 0)
                                                                      .WithCloseBracket(index + 1 == additionalConditions.Length)
                                                                      .WithCombination(index == 0 ? QueryCombination.And : QueryCombination.Or)).ToArray());
            return instance;
        }

        public static T OrAll<T>(this T instance, params IQueryConditionBuilder[] additionalConditions)
            where T : ISingleEntityQueryBuilderBase
        {
            instance.Where(additionalConditions.Select((a, index) => a.WithOpenBracket(index == 0)
                                                                      .WithCloseBracket(index + 1 == additionalConditions.Length)
                                                                      .WithCombination(index == 0 ? QueryCombination.Or : QueryCombination.And)).ToArray());
            return instance;
        }

        public static T OrderBy<T>(this T instance, params IQuerySortOrderBuilder[] additionalOrderByFields)
            where T : ISingleEntityQueryBuilderBase
        {
            instance.OrderByFields.AddRange(additionalOrderByFields);
            return instance;
        }

        public static T OrderBy<T>(this T instance, params string[] additionalSortOrders)
            where T : ISingleEntityQueryBuilderBase
            => instance.OrderBy(additionalSortOrders.Select(s => new QuerySortOrderBuilder().WithField(s)).ToArray());

        public static T OrderBy<T>(this T instance, params IQueryExpressionBuilder[] additionalSortOrders)
            where T : ISingleEntityQueryBuilderBase
                => instance.OrderBy(additionalSortOrders.Select(s => new QuerySortOrderBuilder(new QuerySortOrder(s.Build(), QuerySortOrderDirection.Ascending))).ToArray());

        public static T OrderByDescending<T>(this T instance, params IQuerySortOrderBuilder[] additionalSortOrders)
            where T : ISingleEntityQueryBuilderBase
            => instance.OrderBy(additionalSortOrders.Select(so => new QuerySortOrderBuilder(new QuerySortOrder(so.Field.Build(), QuerySortOrderDirection.Descending))).ToArray());

        public static T OrderByDescending<T>(this T instance, params string[] additionalSortOrders)
            where T : ISingleEntityQueryBuilderBase
            => instance.OrderBy(additionalSortOrders.Select(s => new QuerySortOrderBuilder(new QuerySortOrder(new QueryExpression(s, null), QuerySortOrderDirection.Descending))).ToArray());

        public static T OrderByDescending<T>(this T instance, params IQueryExpressionBuilder[] additionalSortOrders)
            where T : ISingleEntityQueryBuilderBase
            => instance.OrderBy(additionalSortOrders.Select(s => new QuerySortOrderBuilder(new QuerySortOrder(s.Build(), QuerySortOrderDirection.Descending))).ToArray());

        public static T ThenBy<T>(this T instance, params IQuerySortOrderBuilder[] additionalSortOrders)
            where T : ISingleEntityQueryBuilderBase
            => instance.OrderBy(additionalSortOrders);

        public static T ThenBy<T>(this T instance, params string[] additionalSortOrders)
            where T : ISingleEntityQueryBuilderBase
            => instance.OrderBy(additionalSortOrders);

        public static T ThenBy<T>(this T instance, params IQueryExpressionBuilder[] additionalSortOrders)
            where T : ISingleEntityQueryBuilderBase
            => instance.OrderBy(additionalSortOrders);

        public static T ThenByDescending<T>(this T instance, params IQuerySortOrderBuilder[] additionalSortOrders)
            where T : ISingleEntityQueryBuilderBase
            => instance.OrderByDescending(additionalSortOrders);

        public static T ThenByDescending<T>(this T instance, params string[] additionalSortOrders)
            where T : ISingleEntityQueryBuilderBase
            => instance.OrderByDescending(additionalSortOrders);

        public static T ThenByDescending<T>(this T instance, params IQueryExpressionBuilder[] additionalSortOrders)
            where T : ISingleEntityQueryBuilderBase
            => instance.OrderByDescending(additionalSortOrders);

        public static T Offset<T>(this T instance, int? offset)
            where T : ISingleEntityQueryBuilderBase
        {
            instance.Offset = offset;
            return instance;
        }

        public static T Limit<T>(this T instance, int? limit)
            where T : ISingleEntityQueryBuilderBase
        {
            instance.Limit = limit;
            return instance;
        }

        public static T Skip<T>(this T instance, int? offset)
            where T : ISingleEntityQueryBuilderBase
            => instance.Offset(offset);

        public static T Take<T>(this T instance, int? limit)
            where T : ISingleEntityQueryBuilderBase
            => instance.Limit(limit);
    }
}
