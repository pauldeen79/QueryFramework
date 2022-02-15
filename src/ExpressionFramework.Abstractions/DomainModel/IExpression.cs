namespace ExpressionFramework.Abstractions.DomainModel;

public interface IExpression
{
    IExpressionFunction? Function { get; }
    IExpressionBuilder ToBuilder();
}
