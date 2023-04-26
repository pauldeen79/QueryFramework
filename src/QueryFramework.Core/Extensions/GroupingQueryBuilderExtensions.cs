namespace QueryFramework.Core.Extensions;

public static class GroupingQueryBuilderExtensions
{
    public static T GroupBy<T>(this T instance, params ExpressionBuilder[] additionalFieldNames)
        where T : IGroupingQueryBuilder
        => instance.With(x=> x.GroupByFields.AddRange(additionalFieldNames));

    public static T GroupBy<T>(this T instance, params string[] additionalFieldNames)
        where T : IGroupingQueryBuilder
        => instance.GroupBy(additionalFieldNames.Select(s => new FieldExpressionBuilder().WithFieldName(s)));

    public static T GroupBy<T>(this T instance, IEnumerable<string> additionalFieldNames)
        where T : IGroupingQueryBuilder
        => instance.GroupBy(additionalFieldNames.ToArray());

    public static T GroupBy<T>(this T instance, IEnumerable<ExpressionBuilder> additionalFieldNames)
        where T : IGroupingQueryBuilder
        => instance.GroupBy(additionalFieldNames.ToArray());

    public static T Having<T>(this T instance, params ComposableEvaluatableBuilder[] additionalFieldNames)
        where T : IGroupingQueryBuilder
        => instance.With(x => x.GroupByFilter.AddConditions(additionalFieldNames));

    public static T Having<T>(this T instance, IEnumerable<ComposableEvaluatableBuilder> additionalFieldNames)
        where T : IGroupingQueryBuilder
        => instance.Having(additionalFieldNames.ToArray());
}
