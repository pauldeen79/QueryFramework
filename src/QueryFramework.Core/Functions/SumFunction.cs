namespace QueryFramework.Core.Functions;

public record SumFunction : IQueryExpressionFunction
{
    public SumFunction() { }

    public SumFunction(IQueryExpressionFunction? innerFunction)
        => InnerFunction = innerFunction;

    public IQueryExpressionFunction? InnerFunction { get; }

    public IQueryExpressionFunctionBuilder ToBuilder()
        => new SumFunctionBuilder().WithInnerFunction(InnerFunction?.ToBuilder());
}
