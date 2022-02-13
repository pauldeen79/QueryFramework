namespace ExpressionFramework.Abstractions;

public interface IExpressionEvaluator
{
    bool TryEvaluate(object item, IExpression expression, out object? result);
}
