namespace ExpressionFramework.Abstractions;

public interface IFunctionEvaluator
{
    bool TryEvaluate(IExpressionFunction function, object? value, string fieldName, out object? result);
}
