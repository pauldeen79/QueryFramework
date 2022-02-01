namespace QueryFramework.Core.Functions;

public class TrimFunctionBuilder : IQueryExpressionFunctionBuilder
{
    public IQueryExpressionFunctionBuilder? InnerFunction { get; set; }

    public IQueryExpressionFunction Build()
        => new TrimFunction(InnerFunction?.Build());
}
