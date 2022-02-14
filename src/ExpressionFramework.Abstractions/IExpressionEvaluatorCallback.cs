namespace ExpressionFramework.Abstractions;

public interface IExpressionEvaluatorCallback
{
    object? Evaluate(object? item, IExpression expression);
}
