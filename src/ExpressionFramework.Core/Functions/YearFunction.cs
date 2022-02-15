namespace ExpressionFramework.Core.Functions;

public record YearFunction : IExpressionFunction
{
    public YearFunction(IExpressionFunction? innerFunction)
        => InnerFunction = innerFunction;

    public IExpressionFunction? InnerFunction { get; }

    public IExpressionFunctionBuilder ToBuilder()
        => new YearFunctionBuilder().WithInnerFunction(InnerFunction?.ToBuilder());
}
