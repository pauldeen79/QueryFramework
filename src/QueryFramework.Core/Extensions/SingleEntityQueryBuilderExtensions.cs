namespace QueryFramework.Core.Extensions;

public static class SingleEntityQueryBuilderExtensions
{
    public static T Where<T>(this T instance, params IConditionBuilder[] additionalConditions)
        where T : ISingleEntityQueryBuilderBase
        => instance.Chain(x => x.Conditions.AddRange(additionalConditions));

    public static T Where<T>(this T instance, IEnumerable<IConditionBuilder> additionalConditions)
        where T : ISingleEntityQueryBuilderBase
        => instance.Where(additionalConditions.ToArray());

    public static T And<T>(this T instance, params IConditionBuilder[] additionalConditions)
        where T : ISingleEntityQueryBuilderBase
        => instance.Where(additionalConditions);

    public static T And<T>(this T instance, IEnumerable<IConditionBuilder> additionalConditions)
        where T : ISingleEntityQueryBuilderBase
        => instance.Where(additionalConditions);

    public static T OrderBy<T>(this T instance, params IQuerySortOrderBuilder[] additionalOrderByFields)
        where T : ISingleEntityQueryBuilderBase
        => instance.Chain(x => x.OrderByFields.AddRange(additionalOrderByFields));

    public static T OrderBy<T>(this T instance, IEnumerable<IQuerySortOrderBuilder> additionalOrderByFields)
        where T : ISingleEntityQueryBuilderBase
        => instance.OrderBy(additionalOrderByFields.ToArray());

    public static T OrderBy<T>(this T instance, params string[] additionalSortOrders)
        where T : ISingleEntityQueryBuilderBase
        => instance.OrderBy(additionalSortOrders.Select(s => new QuerySortOrderBuilder().WithField(s)));

    public static T OrderBy<T>(this T instance, params IExpressionBuilder[] additionalSortOrders)
        where T : ISingleEntityQueryBuilderBase
            => instance.OrderBy(additionalSortOrders.Select(s => new QuerySortOrderBuilder(new QuerySortOrder(s.Build(), QuerySortOrderDirection.Ascending))));

    public static T OrderByDescending<T>(this T instance, params IQuerySortOrderBuilder[] additionalSortOrders)
        where T : ISingleEntityQueryBuilderBase
        => instance.OrderBy(additionalSortOrders.Select(so => new QuerySortOrderBuilder(new QuerySortOrder(so.Field.Build(), QuerySortOrderDirection.Descending))));

    public static T OrderByDescending<T>(this T instance, params string[] additionalSortOrders)
        where T : ISingleEntityQueryBuilderBase
        => instance.OrderBy(additionalSortOrders.Select(s => new QuerySortOrderBuilder(new QuerySortOrder(new FieldExpression(s, null), QuerySortOrderDirection.Descending))));

    public static T OrderByDescending<T>(this T instance, params IExpressionBuilder[] additionalSortOrders)
        where T : ISingleEntityQueryBuilderBase
        => instance.OrderBy(additionalSortOrders.Select(s => new QuerySortOrderBuilder(new QuerySortOrder(s.Build(), QuerySortOrderDirection.Descending))));

    public static T ThenBy<T>(this T instance, params IQuerySortOrderBuilder[] additionalSortOrders)
        where T : ISingleEntityQueryBuilderBase
        => instance.OrderBy(additionalSortOrders);

    public static T ThenBy<T>(this T instance, params string[] additionalSortOrders)
        where T : ISingleEntityQueryBuilderBase
        => instance.OrderBy(additionalSortOrders);

    public static T ThenBy<T>(this T instance, params IExpressionBuilder[] additionalSortOrders)
        where T : ISingleEntityQueryBuilderBase
        => instance.OrderBy(additionalSortOrders);

    public static T ThenByDescending<T>(this T instance, params IQuerySortOrderBuilder[] additionalSortOrders)
        where T : ISingleEntityQueryBuilderBase
        => instance.OrderByDescending(additionalSortOrders);

    public static T ThenByDescending<T>(this T instance, params string[] additionalSortOrders)
        where T : ISingleEntityQueryBuilderBase
        => instance.OrderByDescending(additionalSortOrders);

    public static T ThenByDescending<T>(this T instance, params IExpressionBuilder[] additionalSortOrders)
        where T : ISingleEntityQueryBuilderBase
        => instance.OrderByDescending(additionalSortOrders);

    public static T Offset<T>(this T instance, int? offset)
        where T : ISingleEntityQueryBuilderBase
        => instance.Chain(x => x.Offset = offset);

    public static T Limit<T>(this T instance, int? limit)
        where T : ISingleEntityQueryBuilderBase
        => instance.Chain(x => x.Limit = limit);

    public static T Skip<T>(this T instance, int? offset)
        where T : ISingleEntityQueryBuilderBase
        => instance.Offset(offset);

    public static T Take<T>(this T instance, int? limit)
        where T : ISingleEntityQueryBuilderBase
        => instance.Limit(limit);
}
