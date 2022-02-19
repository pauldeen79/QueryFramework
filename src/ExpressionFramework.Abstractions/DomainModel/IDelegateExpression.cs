namespace ExpressionFramework.Abstractions.DomainModel;

public interface IDelegateExpression : IExpression
{
    Func<object?, IExpression, IExpressionEvaluator, object?> ValueDelegate { get; }
}
