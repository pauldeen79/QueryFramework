namespace QueryFramework.Abstractions.Extensions;

public static class QuerySortOrderBuilderExtensions
{
    public static IQuerySortOrderBuilder WithFieldName(this IQuerySortOrderBuilder instance, string fieldName)
        => instance.WithFieldNameExpression(new TypedFieldExpressionBuilder<string>().WithExpression(new ContextExpressionBuilder()).WithFieldName(fieldName));
}
