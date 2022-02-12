namespace ExpressionFramework.Core.Functions;

public record DayFunction : IExpressionFunction
{
    public DayFunction() { }

    public DayFunction(IExpressionFunction? innerFunction)
        => InnerFunction = innerFunction;

    public IExpressionFunction? InnerFunction { get; }

    public IExpressionFunctionBuilder ToBuilder()
        => new DayFunctionBuilder().WithInnerFunction(InnerFunction?.ToBuilder());
}
