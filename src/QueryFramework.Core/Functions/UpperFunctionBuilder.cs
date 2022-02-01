namespace QueryFramework.Core.Functions;

public class UpperFunctionBuilder : IQueryExpressionFunctionBuilder
{
    public IQueryExpressionFunctionBuilder? InnerFunction { get; set; }

    public IQueryExpressionFunction Build()
        => new UpperFunction(InnerFunction?.Build());
}
