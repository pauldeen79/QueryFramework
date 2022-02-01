namespace QueryFramework.Core.Functions;

public class MonthFunctionBuilder : IQueryExpressionFunctionBuilder
{
    public IQueryExpressionFunctionBuilder? InnerFunction { get; set; }

    public IQueryExpressionFunction Build()
        => new MonthFunction(InnerFunction?.Build());
}
