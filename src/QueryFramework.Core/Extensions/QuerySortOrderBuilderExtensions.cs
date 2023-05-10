namespace QueryFramework.Abstractions.Extensions;

public static class QuerySortOrderBuilderExtensions
{
    public static QuerySortOrderBuilder WithFieldName(this QuerySortOrderBuilder instance, string fieldName)
        => instance.WithFieldNameExpression(new TypedConstantExpressionBuilder<string>().WithValue(fieldName));
}
