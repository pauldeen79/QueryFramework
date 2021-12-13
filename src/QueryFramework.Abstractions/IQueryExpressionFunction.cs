namespace QueryFramework.Abstractions
{
    public interface IQueryExpressionFunction
    {
        string Expression { get; }
        IQueryExpressionFunction? InnerFunction { get; }
    }
}
