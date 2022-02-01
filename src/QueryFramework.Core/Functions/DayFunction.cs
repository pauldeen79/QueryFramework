namespace QueryFramework.Core.Functions;

public record DayFunction : IQueryExpressionFunction
{
    public DayFunction() { }

    public DayFunction(IQueryExpressionFunction? innerFunction)
        => InnerFunction = innerFunction;

    public IQueryExpressionFunction? InnerFunction { get; }

    public IQueryExpressionFunctionBuilder ToBuilder()
        => new DayFunctionBuilder().WithInnerFunction(InnerFunction?.ToBuilder());
}
