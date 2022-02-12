namespace ExpressionFramework.Core.Functions;

public class LengthFunctionBuilder : IExpressionFunctionBuilder
{
    public IExpressionFunctionBuilder? InnerFunction { get; set; }

    public IExpressionFunction Build()
        => new LengthFunction(InnerFunction?.Build());
}
