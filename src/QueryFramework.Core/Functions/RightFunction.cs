namespace QueryFramework.Core.Functions;

public record RightFunction : IQueryExpressionFunction
{
    public RightFunction(int length)
        => Length = length;

    public RightFunction(int length, IQueryExpressionFunction? innerFunction)
    {
        Length = length;
        InnerFunction = innerFunction;
    }

    public int Length { get; }
    public IQueryExpressionFunction? InnerFunction { get; }

    public IQueryExpressionFunctionBuilder ToBuilder()
        => new RightFunctionBuilder().WithLength(Length).WithInnerFunction(InnerFunction?.ToBuilder());
}
