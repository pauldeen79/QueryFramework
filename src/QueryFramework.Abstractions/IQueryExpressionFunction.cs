namespace QueryFramework.Abstractions
{
    public interface IQueryExpressionFunction
    {
        IQueryExpressionFunction? InnerFunction { get; }
    }
}
