namespace ExpressionFramework.Core.Functions;

public record LeftFunction : IExpressionFunction
{
    public LeftFunction(int length)
        => Length = length;

    public LeftFunction(int length, IExpressionFunction? innerFunction)
    {
        Length = length;
        InnerFunction = innerFunction;
    }

    public int Length { get; }
    public IExpressionFunction? InnerFunction { get; }

    public IExpressionFunctionBuilder ToBuilder()
        => new LeftFunctionBuilder().WithLength(Length).WithInnerFunction(InnerFunction?.ToBuilder());
}
