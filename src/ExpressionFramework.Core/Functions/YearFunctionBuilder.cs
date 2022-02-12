namespace ExpressionFramework.Core.Functions;

public class YearFunctionBuilder : IExpressionFunctionBuilder
{
    public IExpressionFunctionBuilder? InnerFunction { get; set; }

    public IExpressionFunction Build()
        => new YearFunction(InnerFunction?.Build());
}
