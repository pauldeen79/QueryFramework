namespace QueryFramework.Abstractions.Builders
{
    public interface IQueryExpressionFunctionBuilder
    {
        IQueryExpressionFunctionBuilder? InnerFunction { get; set; }

        IQueryExpressionFunction Build();
    }
}
