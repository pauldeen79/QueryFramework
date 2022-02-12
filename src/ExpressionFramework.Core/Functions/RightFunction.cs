namespace ExpressionFramework.Core.Functions;

public record RightFunction : IExpressionFunction
{
    public RightFunction(int length)
        => Length = length;

    public RightFunction(int length, IExpressionFunction? innerFunction)
    {
        Length = length;
        InnerFunction = innerFunction;
    }

    public int Length { get; }
    public IExpressionFunction? InnerFunction { get; }

    public IExpressionFunctionBuilder ToBuilder()
        => new RightFunctionBuilder().WithLength(Length).WithInnerFunction(InnerFunction?.ToBuilder());
}
