namespace ExpressionFramework.Abstractions.DomainModel;

public interface IConstantExpression : IExpression
{
    object? Value { get; }
}
