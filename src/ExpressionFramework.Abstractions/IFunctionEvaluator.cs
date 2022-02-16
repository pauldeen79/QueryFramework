namespace ExpressionFramework.Abstractions;

public interface IFunctionEvaluator
{
    bool TryEvaluate(IExpressionFunction function, object? value, IExpressionEvaluator evaluator, out object? result);
}
