namespace ExpressionFramework.Core.Functions;

public class RightFunctionBuilder : IExpressionFunctionBuilder
{
    public int Length { get; set; }
    public IExpressionFunctionBuilder? InnerFunction { get; set; }

    public RightFunctionBuilder WithLength(int length)
        => this.Chain(x => x.Length = length);

    public IExpressionFunction Build()
        => new RightFunction(Length, InnerFunction?.Build());
}
