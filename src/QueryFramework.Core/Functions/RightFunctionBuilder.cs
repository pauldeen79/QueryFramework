namespace QueryFramework.Core.Functions;

public class RightFunctionBuilder : IQueryExpressionFunctionBuilder
{
    public int Length { get; set; }
    public IQueryExpressionFunctionBuilder? InnerFunction { get; set; }

    public RightFunctionBuilder WithLength(int length)
        => this.Chain(x => x.Length = length);

    public IQueryExpressionFunction Build()
        => new RightFunction(Length, InnerFunction?.Build());
}
