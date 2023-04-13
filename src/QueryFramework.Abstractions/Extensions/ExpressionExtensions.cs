namespace QueryFramework.Abstractions.Extensions;

public static class ExpressionExtensions
{
    public static string GetFieldName(this Expression instance, object? context = null)
        => instance.TryGetFieldName(context) ?? throw new NotSupportedException($"Expression type {instance.GetType().FullName} is not supported. Only ConstantExpression and DelegateExpression are supported");
    
    public static string? TryGetFieldName(this Expression instance, object? context = null)
        => instance is FieldExpression f
            ? TryGetValue(f.FieldNameExpression, context)
            : null;

    private static string? TryGetValue(Expression fieldNameExpression, object? context)
    {
        if (fieldNameExpression is ConstantExpression c)
        {
            return c.Value?.ToString();
        }

        if (fieldNameExpression is DelegateExpression d)
        {
            return d.Value(context)?.ToString();
        }

        return default;
    }
}
