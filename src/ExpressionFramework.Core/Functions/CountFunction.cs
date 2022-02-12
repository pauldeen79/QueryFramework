namespace ExpressionFramework.Core.Functions;

public record CountFunction : IExpressionFunction
{
    public CountFunction() { }

    public CountFunction(IExpressionFunction? innerFunction)
        => InnerFunction = innerFunction;

    public IExpressionFunction? InnerFunction { get; }

    public IExpressionFunctionBuilder ToBuilder()
        => new CountFunctionBuilder().WithInnerFunction(InnerFunction?.ToBuilder());
}
