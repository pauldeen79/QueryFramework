namespace QueryFramework.Core.Functions;

public record CountFunction : IQueryExpressionFunction
{
    public CountFunction() { }

    public CountFunction(IQueryExpressionFunction? innerFunction)
        => InnerFunction = innerFunction;

    public IQueryExpressionFunction? InnerFunction { get; }

    public IQueryExpressionFunctionBuilder ToBuilder()
        => new CountFunctionBuilder().WithInnerFunction(InnerFunction?.ToBuilder());
}
