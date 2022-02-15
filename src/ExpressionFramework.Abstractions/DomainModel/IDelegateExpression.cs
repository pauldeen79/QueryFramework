namespace ExpressionFramework.Abstractions.DomainModel;

public interface IDelegateExpression : IExpression
{
    Func<object?> ValueDelegate { get; }
}
