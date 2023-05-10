namespace QueryFramework.Abstractions.Extensions;

public static class ExpressionExtensions
{
    public static string GetFieldName<T>(this ITypedExpression<T> instance, object? context = null)
        => instance.ToUntyped().GetFieldName(context);

    public static string GetFieldName(this Expression instance, object? context = null)
        => instance.TryGetFieldName(context)
        ?? throw new NotSupportedException($"Expression type {instance.GetType().FullName} is not supported. Only ConstantExpression and DelegateExpression are supported");
    
    public static string? TryGetFieldName(this Expression instance, object? context = null)
        => instance switch
        {
            ConstantExpression c => c.Value?.ToString(),
            DelegateExpression d => d.Value(context)?.ToString(),
            FieldExpression f => f.FieldNameExpression.TryGetValue(context),
            TypedFieldExpression<string> t => t.FieldNameExpression.TryGetValue(context),
            TypedConstantExpression<string> c => c.Value,
            TypedDelegateExpression<string> d => d.Value(context),
            _ => null
        };

    public static Expression? TryGetInnerExpression(this Expression instance)
        => instance.GetPrimaryExpression().Value;

    public static string? TryGetValue(this Expression expression, object? context)
        => expression.Evaluate(context).Value?.ToString();

    public static T? TryGetValue<T>(this ITypedExpression<T> expression, object? context)
        => expression.EvaluateTyped(context).Value;
}
