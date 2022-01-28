using QueryFramework.Abstractions.Builders;

namespace QueryFramework.Abstractions
{
    public partial interface IQueryExpressionFunction
    {
        IQueryExpressionFunctionBuilder ToBuilder();
    }
}
