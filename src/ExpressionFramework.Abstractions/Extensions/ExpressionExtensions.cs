namespace ExpressionFramework.Abstractions.Extensions;

public static class ExpressionExtensions
{
    public static string? GetFieldName(this IExpression instance)
        => (instance as IFieldExpression)?.FieldName;

    public static object? GetConstantValue(this IExpression instance)
        => (instance as IConstantExpression)?.Value;
}
