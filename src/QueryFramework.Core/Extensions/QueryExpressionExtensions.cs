using QueryFramework.Abstractions;
using QueryFramework.Abstractions.Builders;
using QueryFramework.Core.Builders;

namespace QueryFramework.Core.Extensions
{
    public static class QueryExpressionExtensions
    {
        public static IQueryExpressionBuilder ToBuilder(this IQueryExpression instance)
            => instance is ICustomQueryExpression customQueryExpression
                ? customQueryExpression.CreateBuilder()
                : new QueryExpressionBuilder(instance);
    }
}
