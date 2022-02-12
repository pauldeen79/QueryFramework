namespace ExpressionFramework.Core.Functions;

public record SumFunction : IExpressionFunction
{
    public SumFunction() { }

    public SumFunction(IExpressionFunction? innerFunction)
        => InnerFunction = innerFunction;

    public IExpressionFunction? InnerFunction { get; }

    public IExpressionFunctionBuilder ToBuilder()
        => new SumFunctionBuilder().WithInnerFunction(InnerFunction?.ToBuilder());
}
