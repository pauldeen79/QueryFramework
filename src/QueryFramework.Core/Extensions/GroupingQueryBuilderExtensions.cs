namespace QueryFramework.Core.Extensions;

public static class GroupingQueryBuilderExtensions
{
    public static T GroupBy<T>(this T instance, params IQueryExpressionBuilder[] additionalFieldNames)
        where T : IGroupingQueryBuilder
        => instance.Chain(x=> x.GroupByFields.AddRange(additionalFieldNames));

    public static T GroupBy<T>(this T instance, params string[] additionalFieldNames)
        where T : IGroupingQueryBuilder
        => instance.GroupBy(additionalFieldNames.Select(s => new QueryExpressionBuilder().WithFieldName(s)));

    public static T GroupBy<T>(this T instance, IEnumerable<IQueryExpressionBuilder> additionalFieldNames)
        where T : IGroupingQueryBuilder
        => instance.GroupBy(additionalFieldNames.ToArray());

    public static T Having<T>(this T instance, params IQueryConditionBuilder[] additionalFieldNames)
        where T : IGroupingQueryBuilder
        => instance.Chain(x => x.HavingFields.AddRange(additionalFieldNames));

    public static T Having<T>(this T instance, IEnumerable<IQueryConditionBuilder> additionalFieldNames)
        where T : IGroupingQueryBuilder
        => instance.Having(additionalFieldNames.ToArray());
}
