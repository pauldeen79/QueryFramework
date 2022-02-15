namespace ExpressionFramework.Core.Functions;

public record MonthFunction : IExpressionFunction
{
    public MonthFunction(IExpressionFunction? innerFunction)
        => InnerFunction = innerFunction;

    public IExpressionFunction? InnerFunction { get; }

    public IExpressionFunctionBuilder ToBuilder()
        => new MonthFunctionBuilder().WithInnerFunction(InnerFunction?.ToBuilder());
}
