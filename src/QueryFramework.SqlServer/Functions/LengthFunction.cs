using QueryFramework.Abstractions;
using QueryFramework.SqlServer.Extensions;

namespace QueryFramework.SqlServer.Functions
{
    public record LengthFunction : IQueryExpressionFunction
    {
        public LengthFunction() { }

        public LengthFunction(IQueryExpressionFunction? innerFunction) => InnerFunction = innerFunction;

        public string Expression => InnerFunction.GetExpression("LEN({0})");

        public IQueryExpressionFunction? InnerFunction { get; }
    }
}
