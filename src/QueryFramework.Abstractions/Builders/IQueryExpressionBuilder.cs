namespace QueryFramework.Abstractions.Builders
{
    public interface IQueryExpressionBuilder
    {
        string? Expression { get; set; }
        string FieldName { get; set; }

        IQueryExpression Build();
    }
}
