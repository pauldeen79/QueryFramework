namespace QueryFramework.Core.Functions;

public class YearFunctionBuilder : IQueryExpressionFunctionBuilder
{
    public IQueryExpressionFunctionBuilder? InnerFunction { get; set; }

    public IQueryExpressionFunction Build()
        => new YearFunction(InnerFunction?.Build());
}
