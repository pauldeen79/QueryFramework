namespace QueryFramework.Core.Extensions;

public static class SingleEntityQueryBuilderExtensions
{
    public static T Where<T>(this T instance, params ComposableEvaluatableBuilder[] additionalConditions)
        where T : ISingleEntityQueryBuilderBase
        => instance.With(x => x.Filter.AddConditions(additionalConditions));

    public static T Where<T>(this T instance, IEnumerable<ComposableEvaluatableBuilder> additionalConditions)
        where T : ISingleEntityQueryBuilderBase
        => instance.Where(additionalConditions.ToArray());

    public static T Or<T>(this T instance, params ComposableEvaluatableBuilder[] additionalConditions)
        where T : ISingleEntityQueryBuilderBase
        => instance.Where(additionalConditions.Select(a => a.WithCombination(combination: Combination.Or)));

    public static T Or<T>(this T instance, IEnumerable<ComposableEvaluatableBuilder> additionalConditions)
        where T : ISingleEntityQueryBuilderBase
        => instance.Or(additionalConditions.ToArray());

    public static T And<T>(this T instance, params ComposableEvaluatableBuilder[] additionalConditions)
        where T : ISingleEntityQueryBuilderBase
        => instance.Where(additionalConditions.Select(a => a.WithCombination(combination: Combination.And)));

    public static T And<T>(this T instance, IEnumerable<ComposableEvaluatableBuilder> additionalConditions)
        where T : ISingleEntityQueryBuilderBase
        => instance.And(additionalConditions.ToArray());

    public static T AndAny<T>(this T instance, params ComposableEvaluatableBuilder[] additionalConditions)
        where T : ISingleEntityQueryBuilderBase
        => instance.Where(additionalConditions.Select((a, index) => a.WithStartGroup(index == 0)
                                                                     .WithEndGroup(index + 1 == additionalConditions.Length)
                                                                     .WithCombination(index == 0 ? Combination.And : Combination.Or)));

    public static T OrAll<T>(this T instance, params ComposableEvaluatableBuilder[] additionalConditions)
        where T : ISingleEntityQueryBuilderBase
        => instance.Where(additionalConditions.Select((a, index) => a.WithStartGroup(index == 0)
                                                                     .WithEndGroup(index + 1 == additionalConditions.Length)
                                                                     .WithCombination(index == 0 ? Combination.Or : Combination.And)));

    public static T OrderBy<T>(this T instance, params IQuerySortOrderBuilder[] additionalOrderByFields)
        where T : ISingleEntityQueryBuilderBase
        => instance.With(x => x.OrderByFields.AddRange(additionalOrderByFields));

    public static T OrderBy<T>(this T instance, IEnumerable<IQuerySortOrderBuilder> additionalOrderByFields)
        where T : ISingleEntityQueryBuilderBase
        => instance.OrderBy(additionalOrderByFields.ToArray());

    public static T OrderBy<T>(this T instance, params string[] additionalSortOrders)
        where T : ISingleEntityQueryBuilderBase
        => instance.OrderBy(additionalSortOrders.Select(s => new QuerySortOrderBuilder().WithFieldName(s)));

    public static T OrderByDescending<T>(this T instance, params IQuerySortOrderBuilder[] additionalSortOrders)
        where T : ISingleEntityQueryBuilderBase
        => instance.OrderBy(additionalSortOrders.Select(so => new QuerySortOrderBuilder(new QuerySortOrder(so.FieldName.ToString(), QuerySortOrderDirection.Descending))));

    public static T OrderByDescending<T>(this T instance, params string[] additionalSortOrders)
        where T : ISingleEntityQueryBuilderBase
        => instance.OrderBy(additionalSortOrders.Select(s => new QuerySortOrderBuilder(new QuerySortOrder(s, QuerySortOrderDirection.Descending))));

    public static T ThenBy<T>(this T instance, params IQuerySortOrderBuilder[] additionalSortOrders)
        where T : ISingleEntityQueryBuilderBase
        => instance.OrderBy(additionalSortOrders);

    public static T ThenBy<T>(this T instance, params string[] additionalSortOrders)
        where T : ISingleEntityQueryBuilderBase
        => instance.OrderBy(additionalSortOrders);

    public static T ThenByDescending<T>(this T instance, params IQuerySortOrderBuilder[] additionalSortOrders)
        where T : ISingleEntityQueryBuilderBase
        => instance.OrderByDescending(additionalSortOrders);

    public static T ThenByDescending<T>(this T instance, params string[] additionalSortOrders)
        where T : ISingleEntityQueryBuilderBase
        => instance.OrderByDescending(additionalSortOrders);

    public static T Offset<T>(this T instance, int? offset)
        where T : ISingleEntityQueryBuilderBase
        => instance.With(x => x.Offset = offset);

    public static T Limit<T>(this T instance, int? limit)
        where T : ISingleEntityQueryBuilderBase
        => instance.With(x => x.Limit = limit);

    public static T Skip<T>(this T instance, int? offset)
        where T : ISingleEntityQueryBuilderBase
        => instance.Offset(offset);

    public static T Take<T>(this T instance, int? limit)
        where T : ISingleEntityQueryBuilderBase
        => instance.Limit(limit);
}
