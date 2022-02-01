namespace QueryFramework.Core.Functions;

public class SumFunctionBuilder : IQueryExpressionFunctionBuilder
{
    public IQueryExpressionFunctionBuilder? InnerFunction { get; set; }

    public IQueryExpressionFunction Build()
        => new SumFunction(InnerFunction?.Build());
}
