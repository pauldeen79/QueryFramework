namespace QueryFramework.Core.Functions;

public class DayFunctionBuilder : IQueryExpressionFunctionBuilder
{
    public IQueryExpressionFunctionBuilder? InnerFunction { get; set; }

    public IQueryExpressionFunction Build()
        => new DayFunction(InnerFunction?.Build());
}
