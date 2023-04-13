namespace QueryFramework.Abstractions.Extensions;

public static class FieldExpressionBuilderExtensions
{
    public static FieldExpressionBuilder WithFieldName(this FieldExpressionBuilder instance, string fieldName)
        => instance.WithFieldNameExpression(new ConstantExpressionBuilder().WithValue(fieldName));
}
