namespace QueryFramework.Abstractions.Builders
{
    public interface IQueryExpressionBuilder
    {
        string FieldName { get; set; }
        IQueryExpressionFunction? Function { get; set; }

        IQueryExpression Build();
    }
}
