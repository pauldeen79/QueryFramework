using QueryFramework.Abstractions;
using QueryFramework.SqlServer.Extensions;

namespace QueryFramework.SqlServer.Functions
{
    public record TrimFunction : IQueryExpressionFunction
    {
        public TrimFunction() { }

        public TrimFunction(IQueryExpressionFunction? innerFunction) => InnerFunction = innerFunction;

        public string Expression => InnerFunction.GetExpression("TRIM({0})");

        public IQueryExpressionFunction? InnerFunction { get; }
    }
}
