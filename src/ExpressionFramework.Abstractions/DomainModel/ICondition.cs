namespace ExpressionFramework.Abstractions.DomainModel;

public interface ICondition
{
    bool OpenBracket { get; }
    bool CloseBracket { get; }
    IExpression LeftExpression { get; }
    Operator Operator { get; }
    IExpression RightExpression { get; }
    Combination Combination { get; }
}
