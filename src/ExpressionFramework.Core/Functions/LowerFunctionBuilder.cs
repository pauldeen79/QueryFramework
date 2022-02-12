namespace ExpressionFramework.Core.Functions;

public class LowerFunctionBuilder : IExpressionFunctionBuilder
{
    public IExpressionFunctionBuilder? InnerFunction { get; set; }

    public IExpressionFunction Build()
        => new LowerFunction(InnerFunction?.Build());
}
