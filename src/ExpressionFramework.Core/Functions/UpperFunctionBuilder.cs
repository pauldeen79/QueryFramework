namespace ExpressionFramework.Core.Functions;

public class UpperFunctionBuilder : IExpressionFunctionBuilder
{
    public IExpressionFunctionBuilder? InnerFunction { get; set; }

    public IExpressionFunction Build()
        => new UpperFunction(InnerFunction?.Build());
}
