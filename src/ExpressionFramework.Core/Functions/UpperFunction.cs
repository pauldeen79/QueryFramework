namespace ExpressionFramework.Core.Functions;

public record UpperFunction : IExpressionFunction
{
    public UpperFunction(IExpressionFunction? innerFunction)
        => InnerFunction = innerFunction;

    public IExpressionFunction? InnerFunction { get; }

    public IExpressionFunctionBuilder ToBuilder()
        => new UpperFunctionBuilder().WithInnerFunction(InnerFunction?.ToBuilder());
}
