namespace QueryFramework.Core.Functions;

public class LengthFunctionBuilder : IQueryExpressionFunctionBuilder
{
    public IQueryExpressionFunctionBuilder? InnerFunction { get; set; }

    public IQueryExpressionFunction Build()
        => new LengthFunction(InnerFunction?.Build());
}
