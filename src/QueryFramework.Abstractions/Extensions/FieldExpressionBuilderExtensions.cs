namespace QueryFramework.Abstractions.Extensions;

public static class FieldExpressionBuilderExtensions
{
    public static FieldExpressionBuilder WithFieldName(this FieldExpressionBuilder instance, string fieldName)
        => instance.WithFieldNameExpression(new TypedConstantExpressionBuilder<string>().WithValue(fieldName));

    public static TypedFieldExpressionBuilder<T> WithFieldName<T>(this TypedFieldExpressionBuilder<T> instance, string fieldName)
        => instance.WithFieldNameExpression(new TypedConstantExpressionBuilder<string>().WithValue(fieldName));
}
