namespace QueryFramework.Abstractions.Builders
{
    public interface IQueryExpressionBuilder
    {
        string FieldName { get; set; }
        IQueryExpressionFunctionBuilder? Function { get; set; }

        IQueryExpression Build();
    }
}
