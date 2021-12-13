namespace QueryFramework.Abstractions
{
    public interface IQueryExpression
    {
        string FieldName { get; }
        IQueryExpressionFunction? Function { get; }
    }
}
