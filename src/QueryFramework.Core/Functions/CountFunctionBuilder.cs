namespace QueryFramework.Core.Functions;

public class CountFunctionBuilder : IQueryExpressionFunctionBuilder
{
    public IQueryExpressionFunctionBuilder? InnerFunction { get; set; }

    public IQueryExpressionFunction Build()
        => new CountFunction(InnerFunction?.Build());
}
