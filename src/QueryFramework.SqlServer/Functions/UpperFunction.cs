using QueryFramework.Abstractions;
using QueryFramework.SqlServer.Extensions;

namespace QueryFramework.SqlServer.Functions
{
    public record UpperFunction : IQueryExpressionFunction
    {
        public UpperFunction() { }

        public UpperFunction(IQueryExpressionFunction? innerFunction) => InnerFunction = innerFunction;

        public string Expression => InnerFunction.GetExpression("UPPER({0})");

        public IQueryExpressionFunction? InnerFunction { get; }
    }
}
