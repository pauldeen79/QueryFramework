namespace ExpressionFramework.Abstractions;

public interface IFunctionEvaluator
{
    bool TryEvaluate(IExpressionFunction function, object? value, IExpressionEvaluatorCallback callback, out object? result);
}
