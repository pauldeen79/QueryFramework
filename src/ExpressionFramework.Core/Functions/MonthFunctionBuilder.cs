namespace ExpressionFramework.Core.Functions;

public class MonthFunctionBuilder : IExpressionFunctionBuilder
{
    public IExpressionFunctionBuilder? InnerFunction { get; set; }

    public IExpressionFunction Build()
        => new MonthFunction(InnerFunction?.Build());
}
