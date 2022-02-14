namespace ExpressionFramework.Abstractions;

public interface IExpressionEvaluator
{
    bool TryEvaluate(object? item, IExpression expression, IExpressionEvaluatorCallback callback, out object? result);
}
