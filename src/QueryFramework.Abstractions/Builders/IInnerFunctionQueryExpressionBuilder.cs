namespace QueryFramework.Abstractions.Builders
{
    public interface IInnerFunctionQueryExpressionBuilder : IQueryExpressionBuilder
    {
        IQueryExpressionFunction? InnerFunction { get; set; }
    }
}
