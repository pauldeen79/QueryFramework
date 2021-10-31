namespace QueryFramework.Abstractions
{
    public interface IQueryExpression
    {
        string FieldName { get; }
        string Expression { get; }
    }
}
