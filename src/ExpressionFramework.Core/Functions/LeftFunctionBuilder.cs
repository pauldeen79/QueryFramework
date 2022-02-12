namespace ExpressionFramework.Core.Functions;

public class LeftFunctionBuilder : IExpressionFunctionBuilder
{
    public int Length { get; set; }
    public IExpressionFunctionBuilder? InnerFunction { get; set; }

    public LeftFunctionBuilder WithLength(int length)
        => this.Chain(x => x.Length = length);

    public IExpressionFunction Build()
        => new LeftFunction(Length, InnerFunction?.Build());
}
