namespace ExpressionFramework.Abstractions;

public interface IExpressionEvaluator
{
    object? Evaluate(object? item, IExpression expression);
}
