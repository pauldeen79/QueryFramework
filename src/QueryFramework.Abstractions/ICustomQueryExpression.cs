using QueryFramework.Abstractions.Builders;

namespace QueryFramework.Abstractions
{
    public interface ICustomQueryExpression : IQueryExpression
    {
        IQueryExpressionBuilder CreateBuilder();
    }
}
