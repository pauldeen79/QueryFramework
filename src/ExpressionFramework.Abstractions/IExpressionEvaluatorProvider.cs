namespace ExpressionFramework.Abstractions;

public interface IExpressionEvaluatorProvider
{
    bool TryEvaluate(object? item, IExpression expression, IExpressionEvaluator evaluator, out object? result);
}
