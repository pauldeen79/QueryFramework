using QueryFramework.Abstractions;
using QueryFramework.SqlServer.Extensions;

namespace QueryFramework.SqlServer.Functions
{
    public record MonthFunction : IQueryExpressionFunction
    {
        public MonthFunction() { }

        public MonthFunction(IQueryExpressionFunction? innerFunction) => InnerFunction = innerFunction;

        public string Expression => InnerFunction.GetExpression("MONTH({0})");

        public IQueryExpressionFunction? InnerFunction { get; }
    }
}
