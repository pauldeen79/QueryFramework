namespace ExpressionFramework.Core.Functions;

public class SumFunctionBuilder : IExpressionFunctionBuilder
{
    public IExpressionFunctionBuilder? InnerFunction { get; set; }

    public IExpressionFunction Build()
        => new SumFunction(InnerFunction?.Build());
}
