namespace QueryFramework.Core.Functions;

public class LeftFunctionBuilder : IQueryExpressionFunctionBuilder
{
    public int Length { get; set; }
    public IQueryExpressionFunctionBuilder? InnerFunction { get; set; }

    public LeftFunctionBuilder WithLength(int length)
        => this.Chain(x => x.Length = length);

    public IQueryExpressionFunction Build()
        => new LeftFunction(Length, InnerFunction?.Build());
}
