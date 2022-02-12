namespace ExpressionFramework.Core.Functions;

public class TrimFunctionBuilder : IExpressionFunctionBuilder
{
    public IExpressionFunctionBuilder? InnerFunction { get; set; }

    public IExpressionFunction Build()
        => new TrimFunction(InnerFunction?.Build());
}
