namespace QueryFramework.Abstractions.Extensions;

public static class ExpressionExtensions
{
    public static string GetFieldName(this Expression instance, object? context = null)
        => instance.TryGetFieldName(context)
        ?? throw new NotSupportedException($"Expression type {instance.GetType().FullName} is not supported. Only FieldExpression and TypedFieldExpression<string> are supported");
    
    public static string? TryGetFieldName(this Expression instance, object? context = null)
        => instance switch
        {
            FieldExpression f => f.FieldNameExpression.TryGetValue(context),
            TypedFieldExpression<string> t => t.FieldNameExpression.TryGetValue(context),
            _ => null
        };

    public static string? TryGetValue(this Expression expression, object? context)
        => expression.Evaluate(context).Value?.ToString();

    public static T? TryGetValue<T>(this ITypedExpression<T> expression, object? context)
        => expression.EvaluateTyped(context).Value;
}
