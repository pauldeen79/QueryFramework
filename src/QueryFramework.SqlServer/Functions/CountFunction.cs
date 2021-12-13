using QueryFramework.Abstractions;
using QueryFramework.SqlServer.Extensions;

namespace QueryFramework.SqlServer.Functions
{
    public record CountFunction : IQueryExpressionFunction
    {
        public CountFunction() { }

        public CountFunction(IQueryExpressionFunction? innerFunction) => InnerFunction = innerFunction;

        public string Expression => InnerFunction.GetExpression("COUNT({0})");

        public IQueryExpressionFunction? InnerFunction { get; }
    }
}
