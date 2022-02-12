namespace ExpressionFramework.Core.Functions;

public class CountFunctionBuilder : IExpressionFunctionBuilder
{
    public IExpressionFunctionBuilder? InnerFunction { get; set; }

    public IExpressionFunction Build()
        => new CountFunction(InnerFunction?.Build());
}
