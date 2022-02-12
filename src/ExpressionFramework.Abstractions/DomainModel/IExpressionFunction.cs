namespace ExpressionFramework.Abstractions.DomainModel;

public interface IExpressionFunction
{
    IExpressionFunction? InnerFunction { get; }
    IExpressionFunctionBuilder ToBuilder();
}
