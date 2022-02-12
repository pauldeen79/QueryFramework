namespace ExpressionFramework.Core.Functions;

public class DayFunctionBuilder : IExpressionFunctionBuilder
{
    public IExpressionFunctionBuilder? InnerFunction { get; set; }

    public IExpressionFunction Build()
        => new DayFunction(InnerFunction?.Build());
}
