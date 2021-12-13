using QueryFramework.Abstractions;
using QueryFramework.SqlServer.Extensions;

namespace QueryFramework.SqlServer.Functions
{
    public record LowerFunction : IQueryExpressionFunction
    {
        public LowerFunction() { }

        public LowerFunction(IQueryExpressionFunction? innerFunction) => InnerFunction = innerFunction;

        public string Expression => InnerFunction.GetExpression("LOWER({0})");

        public IQueryExpressionFunction? InnerFunction { get; }
    }
}
