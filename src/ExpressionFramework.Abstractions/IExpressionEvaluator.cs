namespace ExpressionFramework.Abstractions;

public interface IExpressionEvaluator
{
    object? GetValue(object item, IExpression field);
}
