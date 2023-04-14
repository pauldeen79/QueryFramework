namespace QueryFramework.Abstractions.Extensions;

public static class ExpressionExtensions
{
    public static string GetFieldName(this Expression instance, object? context = null)
        => instance.TryGetFieldName(context)
        ?? throw new NotSupportedException($"Expression type {instance.GetType().FullName} is not supported. Only ConstantExpression and DelegateExpression are supported");
    
    public static string? TryGetFieldName(this Expression instance, object? context = null)
        => instance is FieldExpression f
            ? f.FieldNameExpression.TryGetValue(context)
            : null;

    public static Expression? TryGetInnerExpression(this Expression instance)
        => instance switch
        {
            LeftExpression l => l.Expression,
            RightExpression r => r.Expression,
            CountExpression c => c.Expression,
            SumExpression s => s.Expression,
            ToLowerCaseExpression l => l.Expression,
            ToUpperCaseExpression u => u.Expression,
            StringLengthExpression l => l.Expression,
            DayExpression d => d.Expression,
            MonthExpression m => m.Expression,
            YearExpression y => y.Expression,
            TrimExpression t => t.Expression,
            _ => null
        };

    public static string? TryGetValue(this Expression expression, object? context)
    {
        if (expression is ConstantExpression c)
        {
            return c.Value?.ToString();
        }

        if (expression is DelegateExpression d)
        {
            return d.Value(context)?.ToString();
        }

        if (expression is ContextExpression)
        {
            return context?.ToString() ?? string.Empty;
        }

        return default;
    }
}
