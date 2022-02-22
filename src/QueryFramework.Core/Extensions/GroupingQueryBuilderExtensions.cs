namespace QueryFramework.Core.Extensions;

public static class GroupingQueryBuilderExtensions
{
    public static T GroupBy<T>(this T instance, params IExpressionBuilder[] additionalFieldNames)
        where T : IGroupingQueryBuilder
        => instance.Chain(x=> x.GroupByFields.AddRange(additionalFieldNames));

    public static T GroupBy<T>(this T instance, params string[] additionalFieldNames)
        where T : IGroupingQueryBuilder
        => instance.GroupBy(additionalFieldNames.Select(s => new FieldExpressionBuilder().WithFieldName(s)));

    public static T GroupBy<T>(this T instance, IEnumerable<IExpressionBuilder> additionalFieldNames)
        where T : IGroupingQueryBuilder
        => instance.GroupBy(additionalFieldNames.ToArray());

    public static T Having<T>(this T instance, params IConditionBuilder[] additionalFieldNames)
        where T : IGroupingQueryBuilder
        => instance.Chain(x => x.HavingFields.AddRange(additionalFieldNames));

    public static T Having<T>(this T instance, IEnumerable<IConditionBuilder> additionalFieldNames)
        where T : IGroupingQueryBuilder
        => instance.Having(additionalFieldNames.ToArray());
}
