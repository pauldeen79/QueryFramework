namespace ExpressionFramework.Core.Functions;

public record LengthFunction : IExpressionFunction
{
    public LengthFunction(IExpressionFunction? innerFunction)
        => InnerFunction = innerFunction;

    public IExpressionFunction? InnerFunction { get; }

    public IExpressionFunctionBuilder ToBuilder()
        => new LengthFunctionBuilder().WithInnerFunction(InnerFunction?.ToBuilder());
}
