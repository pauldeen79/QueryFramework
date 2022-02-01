namespace QueryFramework.Core.Functions;

public class LowerFunctionBuilder : IQueryExpressionFunctionBuilder
{
    public IQueryExpressionFunctionBuilder? InnerFunction { get; set; }

    public IQueryExpressionFunction Build()
        => new LowerFunction(InnerFunction?.Build());
}
