using QueryFramework.Abstractions;
using QueryFramework.SqlServer.Extensions;

namespace QueryFramework.SqlServer.Functions
{
    public record SumFunction : IQueryExpressionFunction
    {
        public SumFunction() { }

        public SumFunction(IQueryExpressionFunction? innerFunction) => InnerFunction = innerFunction;

        public string Expression => InnerFunction.GetExpression("SUM({0})");

        public IQueryExpressionFunction? InnerFunction { get; }
    }
}
